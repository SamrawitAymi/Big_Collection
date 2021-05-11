using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Models;
using Users.Settings;

namespace Users.Services
{
    //tutorial https://dev.to/moe23/asp-net-core-5-rest-api-authentication-with-jwt-step-by-step-140d
    public class JwtTokenHandler
    {
        private readonly IConfiguration _config;

        public JwtTokenHandler(IConfiguration config)
        {
            this._config = config;
        }

        public string CreateToken(User user, bool isAdmin = false)
        {
            if (user == null)
                return null;

            var expirationDate = DateTime.UtcNow.AddMinutes(int.Parse(_config[AppSettings.JWT_EXPIRE_MINUTES]));
            var credentials = SignCredentials(AppSettings.JWT_KEY);
            var claims = SetTokenClaims(user, isAdmin, expirationDate);
            var token = ConstructJwtToken(claims, credentials, expirationDate);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Creates a new refresh token. The refresh can be used to generate a new JWT token for authorization
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateRefreshToken(User user)
        {
            if (user == null)
                return null;

            var expirationDate = DateTime.UtcNow.AddMonths(int.Parse(_config[AppSettings.JWT_EXPIRE_MONTHS]));
            var credentials = SignCredentials(AppSettings.JWT_REFRESHKEY);
            var claims = SetRefreshTokenClaims(user, expirationDate);
            var token = ConstructJwtToken(claims, credentials, expirationDate);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials SignCredentials(string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[secretKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return credentials;
        }

        private IEnumerable<Claim> SetTokenClaims(User user, bool isAdmin, DateTime expires)
        {
            var claims = new List<Claim>()
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("UserEmail", user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Expiration, expires.ToString()),
                new Claim(ClaimTypes.Role, (isAdmin) ? "Admin" : "User")
            };

            return claims;
        }

        private IEnumerable<Claim> SetRefreshTokenClaims(User user, DateTime expires)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Expiration, expires.ToString())
            };

            return claims;
        }

        private JwtSecurityToken ConstructJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials, DateTime expires)
        {
            var token = new JwtSecurityToken(
                _config[AppSettings.JWT_ISSUER],
                _config[AppSettings.JWT_ISSUER],
                claims,
                null,
                expires,
                credentials
                );

            return token;
        }

        /// <summary>
        /// Validate the JWT-redentials and expiration time
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A ClaimsPrincipal that can be used for checking the Identity.IsAuthenticated property to check if token is valid</returns>
        public ClaimsPrincipal ValidateToken(string token)
        {
            var claimsPrincipal = Validate(token, AppSettings.JWT_KEY);
            return claimsPrincipal;
        }

        /// <summary>
        /// Validate the JWT-refresh tokens redentials and expiration time
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A ClaimsPrincipal that can be used for checking the Identity.IsAuthenticated property to check if token is valid</returns>
        public ClaimsPrincipal ValidateRefreshToken(string token)
        {
            var claimsPrincipal = Validate(token, AppSettings.JWT_REFRESHKEY);
            return claimsPrincipal;
        }

        private TokenValidationParameters TokenValidationSetup(string securityKey)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = _config[AppSettings.JWT_ISSUER],
                ValidAudiences = new[] { _config[AppSettings.JWT_ISSUER] },
                ValidateIssuerSigningKey = true,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[securityKey])),
                ClockSkew = TimeSpan.Zero
            };

            return validationParameters;
        }

        private ClaimsPrincipal Validate(string token, string securityKey)
        {
            try
            {
                var validationParameters = TokenValidationSetup(securityKey);
                var handler = new JwtSecurityTokenHandler();
                var result = handler.ValidateToken(token, validationParameters, out var securityToken);
                return result;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }
    }
}
