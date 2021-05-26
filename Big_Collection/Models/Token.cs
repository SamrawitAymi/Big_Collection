using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RenewTokenModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
