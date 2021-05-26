using Big_Collection.Common;
using Big_Collection.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Big_Collection.Services
{
    public class CookieHandler : ICookieHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        JwtTokenHandler _tokenHandler;

        public CookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            this._tokenHandler = new JwtTokenHandler();
            this._httpContextAccessor = httpContextAccessor;
        }


        public async Task CreateAuthenticationCookieAsync(string content, bool isPersistent = false)
        {
            var jwtClaims = await _tokenHandler.GetJwtTokenClaimsAsync(content);
            var claimsIdentity = new ClaimsIdentity(jwtClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddMonths(2)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task CreateLoginCookiesAsync(LogedInUser user, bool rememberUser)
        {
            var token = user.Token;
            var refreshToken = user.RefreshToken;

            await CreateAuthenticationCookieAsync(token, rememberUser);
            CreatePersistentCookie(Cookies.JWT_REFRESH_TOKEN, refreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, token);
        }

        public void CreatePersistentCookie(string name, string content)
        {
            CookieOptions options = new CookieOptions();

            options.HttpOnly = true;
            options.Secure = true;
            options.SameSite = SameSiteMode.Strict;
            options.Expires = DateTime.UtcNow.AddMonths(2);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, content, options);
        }

        public void CreateSessionCookie(string name, string content)
        {
            _httpContextAccessor.HttpContext.Session.SetString(name, content);
        }

        public void DestroyAllCookies()
        {
            _httpContextAccessor.HttpContext.Session.Remove(Cookies.JWT_SESSION_TOKEN);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(Cookies.JWT_REFRESH_TOKEN);
            _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public async Task<string> GetClaimFromAuthenticationCookieAsync(string claimName)
        {
            var userId = await Task.FromResult(_httpContextAccessor.HttpContext.User.FindFirstValue(claimName));
            return userId;
        }

        public string GetPersistentCookieContent(string name)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[name];
        }

        public string GetSessionCookieContent(string name)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(name);
        }

        public void RenewJwtTokens(TokenModel model)
        {
            CreatePersistentCookie(Cookies.JWT_REFRESH_TOKEN, model.RefreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, model.Token);
        }

        public async Task<bool> ValidateJwtTokenSessionExpirationAsync()
        {
            var token = GetSessionCookieContent(Cookies.JWT_SESSION_TOKEN);

            if (token == null)
                return false;

            return await _tokenHandler.ValidateJwtTokenExpirationDateAsync(token);
        }
    }
}
