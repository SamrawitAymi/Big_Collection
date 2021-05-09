using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Services
{
    //tutorial https://dev.to/moe23/asp-net-core-5-rest-api-authentication-with-jwt-step-by-step-140d
    public class JwtTokenHandler
    {
        private readonly IConfiguration _config;

        public JwtTokenHandler()
        {
        }

        public JwtTokenHandler(IConfiguration config)
        {
            this._config = config;
        }

        public string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = SetTokenClaims(user);
            var token = ConfigureToken(claims, credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim> SetTokenClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("UserEmail", user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            return claims;
        }

        public JwtSecurityToken ConfigureToken(IEnumerable<Claim> claims, SigningCredentials credentials)
        {
            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:TokenExpireMinutes"])),
                signingCredentials: credentials);

            return token;
        }
    }
}
