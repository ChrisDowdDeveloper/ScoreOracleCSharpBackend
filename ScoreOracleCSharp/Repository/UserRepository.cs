using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ScoreOracleCSharp.Helpers;
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

    public async Task<List<User>> GetAllUsersAsync(UserQueryObject query)
    {
        var users = _userManager.Users.AsQueryable();
        if(!string.IsNullOrWhiteSpace(query.UserName))
        {
            users = users.Where(u => u.UserName != null && u.UserName.Contains(query.UserName));
        }

        if(!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if(query.SortBy.Equals("UserName", StringComparison.OrdinalIgnoreCase))
            {
                users = query.IsDescending 
                    ? users.OrderByDescending(u => 
                        u.UserName) 
                    : users.OrderBy(u => 
                        u.UserName);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await users
                    .Include(rec => rec.ReceivedFriendships)
                    .Include(req => req.RequestedFriendships)
                    .Include(g => g.GroupsJoined)
                    .Include(n => n.Notifications)
                    .Include(p => p.PredictionsByPlayer)
                    .Include(us => us.UserScores)
                    .Skip(skipNumber)
                    .Take(query.PageSize)
                    .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Include(rec => rec.ReceivedFriendships)
            .Include(req => req.RequestedFriendships)
            .Include(g => g.GroupsJoined)
            .Include(n => n.Notifications)
            .Include(p => p.PredictionsByPlayer)
            .Include(us => us.UserScores)
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"No user found with ID {userId}");
        return user;
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
}
