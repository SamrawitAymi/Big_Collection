using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Context;
using Users.Models;
using Users.Services;

namespace Users.UnitTest.Context
{
    public class TestUserContext : IDisposable
    {

        public AppSettings AppsettingsConfig { get; private set; }
        public UserDbContext UserDbContext { get; private set; }
        public UserManager<User> UserManager { get; private set; }
        public SignInManager<User> SignInManager { get; private set; }
        public RoleManager<UserRole> RoleManager { get; private set; }
        public JwtTokenHandler JwtTokenHandler { get; private set; }


        public TestUserContext()
        {
            AppsettingsConfig = new AppSettings();
            InitializeContext();
        }

        public void InitializeContext()
        {
            var connectionString = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(connectionString).Options;

            UserDbContext = new UserDbContext(dbOption);
            JwtTokenHandler = new JwtTokenHandler(AppsettingsConfig.Config);
            AddIdentityFramework(connectionString);
        }

        public void AddIdentityFramework(string connectionString)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Add database context and logging or IdentityFramework wont load properly
            serviceCollection.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddLogging();

            // Add IdentityFramework to serviceCollection
            serviceCollection.AddIdentity<User, UserRole>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            UserManager = serviceCollection.BuildServiceProvider()
                .GetService<UserManager<User>>();

            SignInManager = serviceCollection.BuildServiceProvider()
                .GetService<SignInManager<User>>();

            RoleManager = serviceCollection.BuildServiceProvider()
                .GetService<RoleManager<UserRole>>();
        }

        public void Dispose()
        {
            UserDbContext?.Dispose();
            UserManager?.Dispose();
            RoleManager?.Dispose();
        }
    }
}