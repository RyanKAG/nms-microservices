using System;
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
        public string GenerateJwt(string key, string issuer, User user)
        {
            var claims = new[] {    
                new Claim(ClaimTypes.Name, user.UserName),
                // new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier,
                    Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));        
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);           
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims, 
                expires: DateTime.Now.AddMinutes(EXPIRATION_IN_MINUTE), signingCredentials: credentials);        
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);  
        }
    }
}