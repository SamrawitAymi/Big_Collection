using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class LoginResponseModel : TokenModel
    {
        public UserModel User { get; set; }
    }
}
