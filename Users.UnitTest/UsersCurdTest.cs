using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Models;
using Users.Repositories;
using Users.UnitTest.Context;

namespace Users.UnitTest
{
    [TestClass]
    public class UsersCurdTest
    {
        public static IUserRepository UserRepositoryClass { get; set; }
        public static TestUserContext UnitTestContext { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Set up DbContext and IdentityFramework
            UnitTestContext = new TestUserContext();

            UserRepositoryClass = new UserRepository(
                UnitTestContext.UserDbContext,
                UnitTestContext.UserManager,
                UnitTestContext.SignInManager,
                UnitTestContext.RoleManager,
                UnitTestContext.JwtTokenHandler);
        }

        [TestMethod]
        public void CreateUserAsync_RegisterNewUser_ReturnCreatedUser()
        {
            // Arrange
            var user = DummyUsers.UserModel();

            // Act
            var createdUser = UserRepositoryClass.CreateUserAsync(user).Result;

            // Assert
            Assert.IsNotNull(createdUser);
        }

        [TestMethod]
        public void CreateUserAsync_ExistingUser_ReturnNull()
        {
            // Arrange
            var user = new UserRegisterModel()
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                Address = "UnitTest 123",
                City = "UnitTest",
                Zip = "12345",
                PhoneNumber = "070-123 45 67",
                Email = "test@bigcollection.com",
                Password = "Test123!"
            };

            // Act
            var createdUser = UserRepositoryClass.CreateUserAsync(user).Result;

            // Assert
            Assert.IsNull(createdUser);
        }
    }
}
