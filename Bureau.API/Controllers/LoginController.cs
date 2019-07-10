using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Bureau.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bureau.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly BureauContext _context;

        public LoginController(BureauContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post(
            [FromBody]User user,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            bool credentialsValid = false;
            if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
            {
                var userBase = _context.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                credentialsValid = (userBase != null &&
                    user.UserName == userBase.UserName &&
                    user.Password == userBase.Password);
            }

            if (credentialsValid)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserName, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    }
                );

                DateTime createdAt = DateTime.Now;
                DateTime expiresAt = createdAt +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = createdAt,
                    Expires = expiresAt
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = createdAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = expiresAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}