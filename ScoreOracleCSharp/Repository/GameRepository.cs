using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDBContext _context;

        public GameRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Game> CreateAsync(Game gameModel)
        {
            await _context.Games.AddAsync(gameModel);
            await _context.SaveChangesAsync();
            return gameModel;
        }

        public async Task<Game?> DeleteAsync(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if(game == null)
            {
                return null;
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<List<Game>> GetAllAsync(GameQueryObject query)
        {
            var games = _context.Games
                                .Include(g => g.HomeTeam)
                                .Include(g => g.AwayTeam)
                                .Include(g => g.Sport)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.TeamName))
            {
                games = games.Where(g => 
                    (g.HomeTeam != null && g.HomeTeam.Name.Contains(query.TeamName)) || 
                    (g.AwayTeam != null && g.AwayTeam.Name.Contains(query.TeamName))
                );
            }

            if (query.GameDate.Equals(DateOnly.FromDateTime(DateTime.Today)))
            {
                games = games.Where(g => g.GameDate == query.GameDate);
            }

            if (!string.IsNullOrWhiteSpace(query.SportName))
            {
                games = games.Where(g => g.Sport != null && g.Sport.Name.Contains(query.SportName));
            }

            if (!string.IsNullOrWhiteSpace(query.GameStatus))
            {
                if (Enum.TryParse<GameStatus>(query.GameStatus, true, out var status))
                {
                    games = games.Where(g => g.GameStatus == status);
                }
                else
                {
                    throw new ArgumentException("Invalid game status value");
                }
            }

            return await games.Include(p => p.GamePrediction).ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.Include(p => p.GamePrediction).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }

        public async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        public async Task<Game?> UpdateAsync(int id, UpdateGameDto gameDto)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if(game == null)
            {
                return null;
            }
            if (gameDto.HomeTeamId.HasValue) game.HomeTeamId = gameDto.HomeTeamId;
            if (gameDto.AwayTeamId.HasValue) game.AwayTeamId = gameDto.AwayTeamId;
            if (gameDto.GameDate.HasValue) game.GameDate = gameDto.GameDate.Value;
            if (gameDto.GameStatus.HasValue) game.GameStatus = gameDto.GameStatus.Value;
            if (gameDto.SportId.HasValue) game.SportId = gameDto.SportId.Value;

            await _context.SaveChangesAsync();
            return game;
        }
    }
}