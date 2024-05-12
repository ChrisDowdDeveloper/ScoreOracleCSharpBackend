using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Helpers;
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

        public async Task<List<Leaderboard>> GetAllAsync(LeaderboardQueryObject query)
        {
            var leaderboards = _context.Leaderboards.Include(l => l.Sport).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                leaderboards = leaderboards.Where(l => l.Name.Contains(query.Name));
            }

            leaderboards = leaderboards.Where(l => l.Type == LeaderboardType.PUBLIC);

            if (!string.IsNullOrWhiteSpace(query.SportName))
            {
                leaderboards = leaderboards.Where(l => l.Sport != null && l.Sport.Name.Contains(query.SportName));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    leaderboards = query.IsDescending 
                            ? leaderboards.OrderByDescending(l => 
                                l.Name) 
                            : leaderboards.OrderBy(l => 
                                l.Name);
                }

                if (query.SortBy.Equals("SportName"))
                {
                    leaderboards = query.IsDescending 
                            ? leaderboards.OrderByDescending(l => 
                                l.Sport != null ? l.Sport.Name : "") 
                            : leaderboards.OrderBy(l => 
                                l.Sport != null ? l.Sport.Name : "");
                }
            }

            return await leaderboards.Include(l => l.Users).ToListAsync();
        }

        public async Task<Leaderboard?> GetByIdAsync(int id)
        {
            return await _context.Leaderboards.Include(l => l.Users).FirstOrDefaultAsync(i => i.Id == id);
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