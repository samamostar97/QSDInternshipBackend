using Bookinghut.Database;
using Bookinghut.Model;
using Bookinghut.Model.Request;
using Bookinghut.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<List<MUser>> Get([FromQuery] UserSearchRequestdto search)
        {
            return await _service.Get(search);
        }
        [HttpGet("{ID}")]
        public async Task<MUser> GetById(int ID)
        {
            return await _service.GetById(ID);
        }
        [HttpPost]
        public async Task<MUser> Insert(UserUpsertRequestdto request)
        {
            return await _service.Insert(request);
        }
        [HttpPut("{ID}")]
        public async Task<MUser> Update(int ID, UserUpsertRequestdto request)
        {
            return await _service.Update(ID, request);
        }
        [HttpDelete("{ID}")]
        public async Task<bool> Delete(int ID)
        {
            return await _service.Delete(ID);
        }
        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _service.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public ActionResult<AuthenticateResponse> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _service.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!User.OwnsToken(token) && User.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            _service.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _service.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailRequest model)
        {
            _service.VerifyEmail(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _service.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("validate-reset-token")]
        public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
        {
            _service.ValidateResetToken(model);
            return Ok(new { message = "Token is valid" });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _service.ResetPassword(model);
            return Ok(new { message = "Password reset successful, you can now login" });
        }
        //[HttpPost("Authenticate")]
        //public async Task<MUser> Authenticate(UserAuthenticationRequest request)
        //{
        //    return await _service.Authenticate(request);
        //}
        //[HttpPost("Login")]
        //public async Task<MUser> Register(UserUpsertRequestdto request)
        //{
        //    return await _service.Insert(request);
        //}
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
