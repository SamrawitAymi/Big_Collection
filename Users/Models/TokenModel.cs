using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class NewTokenModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
