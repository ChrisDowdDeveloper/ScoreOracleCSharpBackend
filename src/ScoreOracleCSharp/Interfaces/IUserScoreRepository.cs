using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IUserScoreRepository
    {
        Task<List<UserScore>> GetAllAsync(UserScoreQueryObject query);
        Task<UserScore?> GetByIdAsync(int id);
        Task<UserScore> CreateAsync(UserScore userScoreModel);
        Task<UserScore?> UpdateAsync(int id, UpdateUserScoreDto userScoreDto);
        Task<UserScore?> DeleteAsync(int id);
        Task<bool> LeaderboardExists(int leaderboardId);
        Task<bool> UserExists(string userId);
    }
}