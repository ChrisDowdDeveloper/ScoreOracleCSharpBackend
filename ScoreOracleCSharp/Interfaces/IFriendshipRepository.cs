using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<List<Friendship>> GetAllAsync();
        Task<Friendship?> GetByIdAsync(int id);
        Task<Friendship> CreateAsync(Friendship friendshipModel);
        Task<Friendship?> UpdateAsync(int id, UpdateFriendshipDto friendshipDto);
        Task<Friendship?> DeleteAsync(int id);
        Task<bool> UserExists(string userId);
        Task<bool> UserCanDeleteFriendship(string userId, int id);
    }
}