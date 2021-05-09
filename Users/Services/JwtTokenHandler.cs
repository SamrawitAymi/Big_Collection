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
using Users.Settings;

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
            if (user == null)
                return null;

            var expirationDate = DateTime.UtcNow.AddMinutes(int.Parse(_config[AppSettings.JWT_EXPIRE_MINUTES]));
            var credentials = SignCredentials(AppSettings.JWT_KEY);
            var claims = SetTokenClaims(user);
            var token = ConstructJwtToken(claims, credentials, expirationDate);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken(User user)
        {
            if (user == null)
                return null;

            var expirationDate = DateTime.UtcNow.AddMonths(int.Parse(_config[AppSettings.JWT_EXPIRE_MONTHS]));
            var credentials = SignCredentials(AppSettings.JWT_REFRESHKEY);
            var claims = SetRefreshTokenClaims(user);
            var token = ConstructJwtToken(claims, credentials, expirationDate);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials SignCredentials(string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[secretKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return credentials;
        }

        private IEnumerable<Claim> SetTokenClaims(User user)
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

        private IEnumerable<Claim> SetRefreshTokenClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString())
            };

            return claims;
        }

        private JwtSecurityToken ConstructJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials, DateTime expires)
        {
            var token = new JwtSecurityToken(
                issuer: _config[AppSettings.JWT_ISSUER],
                audience: _config[AppSettings.JWT_ISSUER],
                claims,
                expires: expires,
                signingCredentials: credentials);

            return token;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var claimsPrincipal = Validate(token, AppSettings.JWT_KEY);
            return claimsPrincipal;
        }

        public ClaimsPrincipal ValidateRefreshToken(string token)
        {
            var claimsPrincipal = Validate(token, AppSettings.JWT_REFRESHKEY);
            return claimsPrincipal;
        }

        private TokenValidationParameters TokenValidationSetup(string token, string securityKey)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = _config[AppSettings.JWT_ISSUER],
                ValidAudiences = new[] { _config[AppSettings.JWT_ISSUER] },
                ValidateIssuerSigningKey = true,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[securityKey]))
            };

            return validationParameters;
        }

        private ClaimsPrincipal Validate(string token, string securityKey)
        {
            var validationParameters = TokenValidationSetup(token, securityKey);
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ValidateToken(token, validationParameters, out var securityToken);

            return result;
        }
    }
}
