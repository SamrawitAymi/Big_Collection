using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Settings
{
    public static class AppSettings
    {
        public const string JWT_KEY = "JWT:Key";
        public const string JWT_REFRESHKEY = "JWT:RefreshKey";
        public const string JWT_ISSUER = "JWT:Issuer";
        public const string JWT_EXPIRE_MINUTES = "JWT:ExpireMinutes";
        public const string JWT_EXPIRE_MONTHS = "JWT:ExpireMonths";
    }
}
