using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Big_Collection.Services
{
    public interface IJwtTokenHandler
    {
        Task<IEnumerable<Claim>> GetJwtTokenClaimsAsync(string token);
        Task<bool> ValidateJwtTokenExpirationDateAsync(string token);
    }
}
