using Bookinghut.Model;
using Bookinghut.Model.Request;
using Bookinghut.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace Bookinghut.Security
//{
//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private readonly IUserService _korisnikService;

//        public BasicAuthenticationHandler(
//            IOptionsMonitor<AuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock,
//            IUserService korisnikService)
//            : base(options, logger, encoder, clock)
//        {
//            _korisnikService = korisnikService;
//        }

        //protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        //{
        //    if (!Request.Headers.ContainsKey("Authorization"))
        //        return AuthenticateResult.Fail("Missing Authorization Header");

        //    MUser user = null;
        //    try
        //    {
        //        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        //        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        //        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
        //        var request = new UserAuthenticationRequest()
        //        {
        //            Username = credentials[0],
        //            Password = credentials[1]
        //        };

        //        user = await _korisnikService.Authenticate(request);
        //    }
        //    catch
        //    {
        //        return AuthenticateResult.Fail("Invalid Authorization Header");
        //    }

        //    if (user == null)
        //        return AuthenticateResult.Fail("Invalid Username or Password");

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Username),
        //        new Claim(ClaimTypes.Name, user.FirstName),
        //    };

        //    foreach (var role in user.UserRole)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
        //    }


        //    var identity = new ClaimsIdentity(claims, Scheme.Name);
        //    var principal = new ClaimsPrincipal(identity);
        //    var ticket = new AuthenticationTicket(principal, Scheme.Name);

        //    return AuthenticateResult.Success(ticket);
//        //}
//    }
//}
