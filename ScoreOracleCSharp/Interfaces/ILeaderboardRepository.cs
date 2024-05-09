using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<List<Leaderboard>> GetAllAsync();
        Task<Leaderboard?> GetByIdAsync(int id);
        Task <Leaderboard> CreateAsync(Leaderboard leaderboardModel);
        Task<Leaderboard?> UpdateAsync(int id, UpdateLeaderboardDto leaderboardDto);
        Task<Leaderboard?> DeleteAsync(int id);
        Task<bool> SportExists(int sportId);
    }
}