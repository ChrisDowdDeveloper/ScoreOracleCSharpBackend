using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class UserScoreRepository : IUserScoreRepository
    {
        private readonly ApplicationDBContext _context;
        public UserScoreRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserScore> CreateAsync(UserScore userScoreModel)
        {
            await _context.UserScores.AddAsync(userScoreModel);
            await _context.SaveChangesAsync();
            return userScoreModel;
        }

        public async Task<UserScore?> DeleteAsync(int id)
        {
            var userScore = await _context.UserScores.FirstOrDefaultAsync(us => us.Id == id);
            if(userScore == null)
            {
                return null;
            }
            _context.UserScores.Remove(userScore);
            await _context.SaveChangesAsync();
            return userScore;
        }

        public async Task<List<UserScore>> GetAllAsync(UserScoreQueryObject query)
        {
            var userScore = _context.UserScores.Include(us => us.User).AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(query.UserName))
            {
                userScore = userScore.Where(us => us.User != null && us.User.UserName != null && us.User.UserName.Contains(query.UserName));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("UserName", StringComparison.OrdinalIgnoreCase))
                {
                    userScore = query.IsDescending 
                            ? userScore.OrderByDescending(us => 
                                us.User != null ? us.User.UserName : "") 
                            : userScore.OrderBy(us => 
                                us.User != null ? us.User.UserName : "");
                }
            }

            return await userScore.ToListAsync();
        }

        public async Task<UserScore?> GetByIdAsync(int id)
        {
            return await _context.UserScores.FindAsync(id);
        }

        public async Task<bool> LeaderboardExists(int leaderboardId)
        {
            return await _context.Leaderboards.AnyAsync(l => l.Id == leaderboardId);
        }

        public async Task<UserScore?> UpdateAsync(int id, UpdateUserScoreDto userScoreDto)
        {
            var userScore = await _context.UserScores.FirstOrDefaultAsync(us => us.Id == id);
            if(userScore == null)
            {
                return null;
            }
            userScore.Score = userScoreDto.Score;
            userScore.UpdatedLast = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return userScore;
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}