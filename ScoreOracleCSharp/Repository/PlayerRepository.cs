using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{

    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDBContext _context;
        public PlayerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Player> CreateAsync(Player playerModel)
        {
            await _context.Players.AddAsync(playerModel);
            await _context.SaveChangesAsync();
            return playerModel;
        }

        public async Task<Player?> DeleteAsync(int id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
            if(player == null)
            {
                return null;
            }
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public Task<bool> TeamExists(int teamId)
        {
            return _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        public async Task<Player?> UpdateAsync(int id, UpdatePlayerDto playerDto)
        {
            var player = await _context.Players.Include(p => p.PlayerInjury).FirstOrDefaultAsync(p => p.Id == id);
            if (player == null)
            {
                return null;
            }

            if (playerDto.TeamId.HasValue && !await TeamExists(playerDto.TeamId.Value))
            {
                throw new InvalidOperationException("Team does not exist");
            }

            player.TeamId = playerDto.TeamId ?? player.TeamId;
            player.FirstName = playerDto.FirstName;
            player.LastName = playerDto.LastName;
            player.Position = playerDto.Position;

            if (playerDto.IsInjured && !player.PlayerInjury.Any())
            {
                player.PlayerInjury.Add(new Injury { 
                    PlayerId = player.Id,
                    Description = "This player is injured, please update accordingly",
                    TeamId = player.TeamId
                });
            }
            else if (!playerDto.IsInjured)
            {
                player.PlayerInjury.Clear();
            }

            await _context.SaveChangesAsync();
            return player;
        }

    }
}