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
        public static User TestUser()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Emma",
                LastName = "Nike",
                Email = "emma.nike@yahoo.com", 
                UserName = "emma.nike@yahoo.com",
                Address = "TestAddress123",
                City = "TestCity",
                Zip = "12345",
                PhoneNumber = "070-111 23 45"
            };
            return user;
        }

        public static UserRegisterModel UserModel()
        {
            UserRegisterModel user = new UserRegisterModel()
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                Address = "UnitTest 123",
                City = "UnitTest",
                Zip = "12345",
                PhoneNumber = "070-123 45 67",
                Email = "test@bigcollection.se",
                Password = "Test123!"
            };
            return user;
        }
    }
}
