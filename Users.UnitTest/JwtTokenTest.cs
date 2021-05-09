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

        //[ClassInitialize]
        //public static void LoadAppsettings(TestContext context)
        //{
        //    var appSettings = new AppSettings();
        //    Config = appSettings.Config;
        //}


        [TestMethod]
        public void CreateToken_createNewToken_ReturnNull()
        {
            //Arrange
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            JwtTokenHandler createToken = new JwtTokenHandler(config);
            var user = DummyUsers.User();

            // Act
            string token = createToken.CreateToken(user);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void SetTokenClaims_CreateSetOfClaims_ReturnItemsNotNull()
        {
            // Arrange
            JwtTokenHandler tokenHandler = new JwtTokenHandler();
            var user = DummyUsers.User();

            // Act
            var result = tokenHandler.SetTokenClaims(user).ToList();

            // Assert
            CollectionAssert.AllItemsAreNotNull(result);
        }

        [TestMethod]
        public void ConfigureToken_CreateJwtSecurityToken_ReturnJwtSecurityTokenNotNull()
        {
            // Arrange
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtTokenHandler tokenHandler = new JwtTokenHandler(config);
            var user = DummyUsers.User();
            var claims = tokenHandler.SetTokenClaims(user);

            // Act
            var result = tokenHandler.ConfigureToken(claims, credentials);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
