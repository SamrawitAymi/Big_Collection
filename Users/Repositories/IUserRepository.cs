using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUserAsync(UserRegisterModel userRegiModel);
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> DeleteUserAsync(Guid id);
        Task<UserModel> UpdateUserAsync(Guid id, UserModel user);
        Task<LoginResponseModel> LoginUserAsync(LoginModel loginModel);
        Task<UserModel> UpdatePasswordAsync(Guid id, string oldPass, string newPass);
        Task<TokenModel> GenerateNewTokensAsync(Guid userId, string refreshToken);
        Task<bool> CheckIfEmailIsRegisteredAsync(string email);
    }
}
