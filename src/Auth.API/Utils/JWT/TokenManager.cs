using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Utils.JWT
{
    public class TokenManager : ITokenManager
    {
        private const double EXPIRATION_IN_MINUTE = 10;

        public string GenerateJwt(string key, string issuer, User user, IList<string> roles)
        {
            var claims = new[]
            {
                new Claim("UserName", user.UserName),
                // new Claim(ClaimTypes.Role, user.Role),
                new Claim("Id",
                    user.Id.ToString()),
                new Claim("Role", "test"),
                new Claim("Role", "test2"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddMinutes(EXPIRATION_IN_MINUTE), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}