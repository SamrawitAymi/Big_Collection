using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Context;
using Users.Models;
using Users.Services;

namespace Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenHandler _tokenHacndler;

        public UserRepository(UserDbContext userDbContext, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, JwtTokenHandler jwtTokenHandler)
        {
            this._context = userDbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._tokenHacndler = jwtTokenHandler;
        }

        public async Task<UserModel> CreateUserAsync(UserRegisterModel userRegiModel)
        {
            try
            {
                var user = ConvertToUser(userRegiModel);
                var result = await _userManager.CreateAsync(user, userRegiModel.Password);

                if (result.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(user, "User");

                    if (role.Succeeded)
                        return await ConvertToUserModelAsync(user);
                    else
                        await _userManager.DeleteAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }

            return null;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return null;

                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    var userModel = await ConvertToUserModelAsync(user);
                    return userModel;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<UserModel> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserModel>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }


        public Task<UserModel> LoginUserAsync(LoginModel loginModel)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> UpdateUserAsync(Guid id, User user)
        {
            throw new NotImplementedException();
        }


        private async Task<UserModel> ConvertToUserModelAsync(User user)
        {
            var userModel = new UserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                Zip = user.Zip
            };

            return await Task.FromResult(userModel);
        }

        private User ConvertToUser(UserRegisterModel userModel)
        {
            User user = new User()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                PhoneNumber = userModel.PhoneNumber,
                Address = userModel.Address,
                City = userModel.City,
                Email = userModel.Email,
                UserName = userModel.Email,
                Zip = userModel.Zip
            };

            return user;
        }
    }
}
