using Big_Collection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Services
{
    public interface ICookieHandler
    {
        Task CreateLoginCookiesAsync(LogedInUser user, bool rememberUser);
        Task CreateAuthenticationCookieAsync(string content, bool isPersistent = false);
        void CreatePersistentCookie(string name, string content);
        string GetPersistentCookieContent(string name);
        void CreateSessionCookie(string name, string content);
        string GetSessionCookieContent(string name);
        void DestroyAllCookies();
        Task<bool> ValidateJwtTokenSessionExpirationAsync();
        Task<string> GetClaimFromAuthenticationCookieAsync(string claimName);
        void RenewJwtTokens(TokenModel model);
    }
}
