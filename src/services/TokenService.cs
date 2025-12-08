using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using userService.src.models;

namespace userService.src.services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IEnumerable<string>? roles = null);
    }

    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService()
        {
            var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY")
                ?? throw new ArgumentNullException("JWT_SIGNING_KEY environment variable is not set.");
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new ArgumentNullException("JWT_ISSUER environment variable is not set.");
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                ?? throw new ArgumentNullException("JWT_AUDIENCE environment variable is not set.");

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        public string CreateToken(AppUser user, IEnumerable<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roles == null)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }
            else
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds,
                Issuer = _issuer,
                Audience = _audience
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}