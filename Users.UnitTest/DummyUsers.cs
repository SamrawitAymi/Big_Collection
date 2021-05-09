using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Models;

namespace Users.UnitTest
{
    public class DummyUsers
    {
        public static User User()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Emma",
                LastName = "Nike",
                Email = "emma.nike@yahoo.com"
            };
            return user;
        }
    }
}
