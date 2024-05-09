using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class LeaderboardRepository : ILeaderboardRepository
    {

        private readonly ApplicationDBContext _context;
        
        public LeaderboardRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Leaderboard> CreateAsync(Leaderboard leaderboardModel)
        {
            await _context.Leaderboards.AddAsync(leaderboardModel);
            await _context.SaveChangesAsync();
            return leaderboardModel;
        }

        public async Task<Leaderboard?> DeleteAsync(int id)
        {
            var leaderboard = await _context.Leaderboards.FirstOrDefaultAsync(l => l.Id == id);
            if(leaderboard == null)
            {
                return null;
            }
            _context.Leaderboards.Remove(leaderboard);
            await _context.SaveChangesAsync();
            return leaderboard;
        }

        public async Task<List<Leaderboard>> GetAllAsync()
        {
            return await _context.Leaderboards.ToListAsync();
        }

        public async Task<Leaderboard?> GetByIdAsync(int id)
        {
            return await _context.Leaderboards.FindAsync(id);
        }

        public async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }

        public async Task<Leaderboard?> UpdateAsync(int id, UpdateLeaderboardDto leaderboardDto)
        {
            var leaderboard = await _context.Leaderboards.FirstOrDefaultAsync(l => l.Id == id);
            if (leaderboard == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(leaderboardDto.Name))
            {
                leaderboard.Name = leaderboardDto.Name;
            }

            if (!string.IsNullOrWhiteSpace(leaderboardDto.Type) && Enum.TryParse<LeaderboardType>(leaderboardDto.Type, true, out var parsedType))
            {
                leaderboard.Type = parsedType;
            }
            else if (!string.IsNullOrWhiteSpace(leaderboardDto.Type))
            {
                throw new ArgumentException("Invalid leaderboard type specified.");
            }

            if (leaderboardDto.SportId.HasValue && !await SportExists(leaderboardDto.SportId.Value))
            {
                throw new InvalidOperationException("Specified sport does not exist.");
            }
            else if (leaderboardDto.SportId.HasValue)
            {
                leaderboard.SportId = leaderboardDto.SportId.Value;
            }

            await _context.SaveChangesAsync();
            return leaderboard;
        }
    }
}