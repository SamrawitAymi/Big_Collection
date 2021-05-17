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
    }
}
