using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync(UserQueryObject query);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
    }
}