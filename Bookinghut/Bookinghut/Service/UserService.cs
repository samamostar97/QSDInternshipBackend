using AutoMapper;
using Bookinghut.Database;
using Bookinghut.Helper;
using Bookinghut.Model;
using Bookinghut.Model.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Bookinghut.Service
{
    public interface IUserService
    {
        //Task<MUser> Authenticate(UserAuthenticationRequest request);
        //Task<MUser> Login(UserUpsertRequestdto request);
        Task<List<MUser>> Get(UserSearchRequestdto search);
        Task<MUser> GetById(int ID);
        Task<MUser> Insert(UserUpsertRequestdto request);
        Task<MUser> Update(int ID, UserUpsertRequestdto request);
        Task<bool> Delete(int ID);



        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        void Register(RegisterRequest model, string origin);
        void VerifyEmail(string token);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(ValidateResetTokenRequest model);
        void ResetPassword(ResetPasswordRequest model);


    }
    public class UserService : IUserService
    {
        private readonly BookinghutContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public UserService(BookinghutContext context, IMapper mapper, IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<List<MUser>> Get(UserSearchRequestdto search)
        {

            var query = _context.User.AsQueryable();

            if (search.UserID != 0)
            {
                query = query.Where(i => i.UserID== search.UserID);
            }



            var list = await query.ToListAsync();
            return _mapper.Map<List<MUser>>(list);
        }

        public async Task<MUser> GetById(int ID)
        {
            var entity = await _context.User
               .Where(i => i.UserID == ID)
               .SingleOrDefaultAsync();

            return _mapper.Map<MUser>(entity);
        }

        public async Task<MUser> Insert(UserUpsertRequestdto request)
        {
            var entity = _mapper.Map<User>(request);
            _context.Set<User>().Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<MUser>(entity);
        }

        public async Task<MUser> Update(int ID, UserUpsertRequestdto request)
        {
            var entity = _context.Set<User>().Find(ID);
            _context.Set<User>().Attach(entity);
            _context.Set<User>().Update(entity);

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync();

            return _mapper.Map<MUser>(entity);
        }

        public async Task<bool> Delete(int ID)
        {
            var user = await _context.User.Where(i => i.UserID== ID).FirstOrDefaultAsync();
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        //public async Task<MUser> Authenticate(UserAuthenticationRequest request)
        //{
        //    var korisnik = await _context.User
        //        .Include(i => i.UserRole)
        //        .ThenInclude(j => j.Role)
        //        .FirstOrDefaultAsync(i => i.Username == request.Username);

        //    if (korisnik != null)
        //    {
        //        var newHash = GenerateHash(korisnik.PasswordSalt, request.Password);

        //        if (newHash == korisnik.PasswordHash)
        //        {
        //            return _mapper.Map<MUser>(korisnik);
        //        }
        //    }
        //    return null;
        //}
        //public async Task<MUser> Login(UserUpsertRequestdto request)
        //{
        //    if (request.Password != request.PasswordConfirmation)
        //    {
        //        throw new Exception("Passwords do not match!");
        //    }
        //    request.Role = new List<int> { 1, 2 };
        //    var entity = _mapper.Map<User>(request);
        //    entity.PasswordSalt = GenerateSalt();
        //    entity.PasswordHash = GenerateHash(entity.PasswordSalt, request.Password);

        //    await _context.User.AddAsync(entity);
        //    await _context.SaveChangesAsync();

        //    return _mapper.Map<MUser>(entity);
        //}


        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == model.Username);

            if (user == null || !user.IsVerified || !BC.Verify(model.Password, user.PasswordHash))
                throw new Exception("Mail or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(user);

            // save changes to db
            _context.Update(user);
            _context.SaveChanges();

            var response = _mapper.Map<AuthenticateResponse>(user);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, user) = getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(user);

            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            var response = _mapper.Map<AuthenticateResponse>(user);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }
        public void Register(RegisterRequest model, string origin)
        {
            // validate
            if (_context.User.Any(x => x.Mail == model.Mail))
            {
            //    // send already registered error in email to prevent account enumeration
                sendAlreadyRegisteredEmail(model.Mail, origin);
              return;
            }

            // map model to new account object
            var user = _mapper.Map<User>(model);

            // first registered account is an admin
            var isFirstAccount = _context.User.Count() == 0;
            user.Role = isFirstAccount ? Role.Admin : Role.Customer;
            user.Created = DateTime.UtcNow;
            user.VerificationToken = randomTokenString();


            // hash password
            user.PasswordHash = BC.HashPassword(model.Password);

            // save account
            _context.User.Add(user);
            _context.SaveChanges();

            // send email
           sendVerificationEmail(user, origin);
        }
        public void VerifyEmail(string token)
        {
            var user = _context.User.SingleOrDefault(x => x.VerificationToken == token);

            if (user == null) throw new Exception("Verification failed");

            user.Verified = DateTime.UtcNow;
            user.VerificationToken = null;

            _context.User.Update(user);
            _context.SaveChanges();
        }
        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var user = _context.User.SingleOrDefault(x => x.Mail == model.Mail);

            // always return ok response to prevent email enumeration
            if (user == null) return;

            // create reset token that expires after 1 day
            user.ResetToken = randomTokenString();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.User.Update(user);
            _context.SaveChanges();

            // send email
            sendPasswordResetEmail(user, origin);
        }
        public void RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, user) = getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();
        }

        private (RefreshToken, User) getRefreshToken(string token)
        {
            var user = _context.User.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null) throw new Exception("Invalid token");
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new Exception("Invalid token");
            return (refreshToken, user);
        }
        public void ResetPassword(ResetPasswordRequest model)
        {
            var user = _context.User.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
                throw new Exception("Invalid token");

            ////// update password and remove reset token
            user.PasswordHash = BC.HashPassword(model.Password);
            user.PasswordReset = DateTime.UtcNow;
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            _context.User.Update(user);
            _context.SaveChanges();
        }

        private void removeOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserID", user.UserID.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public void ValidateResetToken(ValidateResetTokenRequest model)
        {
            var user = _context.User.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
                throw new Exception("Invalid token");
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void sendVerificationEmail(User user, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/user/verify-email?token={user.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/user/verify-email</code> api route:</p>
                             <p><code>{user.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: user.Mail,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        private void sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/user/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/user/forgot-password</code> api route.</p>";

            _emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );
        }

        private void sendPasswordResetEmail(User user, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/user/reset-password?token={user.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{user.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: user.Mail,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
