using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException($"No user found with ID {userId}");
        return user;
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded) return user;
        throw new InvalidOperationException("Failed to create user");
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return user;
        throw new InvalidOperationException("Failed to update user");
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new KeyNotFoundException($"No user found with email {email}");
        return user;
    }
}
