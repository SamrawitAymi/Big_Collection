using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Models;
using Users.Services;
using Users.UnitTest.Context;

namespace Users.UnitTest
{
    [TestClass]
    public class JwtTokenTest
    {
        public static IConfigurationRoot Config { get; set; }

        [ClassInitialize]
        public static void LoadAppsettings(TestContext context)
        {
            var appSettings = new AppSettings();
            Config = appSettings.Config;
        }


        [TestMethod]
        public void CreateToken_createNewToken_ReturnStringToken()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();

            // Act
            string token = tokenHandler.CreateToken(user, false);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void ValidateToken_CheckIfJwtTokenIsValid_ReturnTrue()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();
            string token = tokenHandler.CreateToken(user);

            // Act
            var result = tokenHandler.ValidateToken(token)
                .Identity
                .IsAuthenticated;

            // Assert
            Assert.IsTrue(result);

        }

       
        [TestMethod]
        public void CreateToken_TryCreateNewJwtTokenWithNullUser_ReturnNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();

            // Act
            string token = tokenHandler.CreateToken(null);

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtToken_ReturnNotNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();

            // Act
            string token = tokenHandler.CreateRefreshToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void CreateRefreshToken_TryCreateNewRefreshJwtTokenWithNullUser_ReturnNULL()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();

            // Act
            string token = tokenHandler.CreateRefreshToken(null);

            // Assert
            Assert.IsNull(token);
        }

     

        [TestMethod]
        public void ValidateRefreshToken_CheckIfJwtRefreshTokenIsValid_ReturnTrue()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler(Config);
            var user = DummyUsers.User();
            string token = tokenHandler.CreateRefreshToken(user);

            // Act
            var result = tokenHandler.ValidateRefreshToken(token)
                .Identity
                .IsAuthenticated;

            // Assert
            Assert.IsTrue(result);

        }
    }
}
