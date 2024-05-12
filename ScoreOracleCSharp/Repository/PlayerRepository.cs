using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Helpers;
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

        public async Task<List<Player>> GetAllAsync(PlayerQueryObject query)
        {
            var players = _context.Players.Include(p => p.Team).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.PlayerName))
            {
                string playerName = query.PlayerName.ToLower();
                players = players.Where(p => ((p.FirstName ?? "") + " " + (p.LastName ?? "")).ToLower().Contains(playerName));
            }

            if(!string.IsNullOrWhiteSpace(query.Position))
            {
                players = players.Where(p => p.Position.Contains(query.Position));
            }

            if(!string.IsNullOrWhiteSpace(query.TeamName))
            {
                players = players.Where(p => p.Team != null && p.Team.Name.Contains(query.TeamName));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("PlayerName", StringComparison.OrdinalIgnoreCase))
                {
                    players = query.IsDescending 
                        ? players.OrderByDescending(p => 
                            (p.FirstName ?? "") + " " + (p.LastName ?? "").ToLower()) 
                        : players.OrderBy(p => 
                            (p.FirstName ?? "") + " " + (p.LastName ?? "").ToLower());  
                }

                if(query.SortBy.Equals("Position", StringComparison.OrdinalIgnoreCase))
                {
                    players = query.IsDescending 
                        ? players.OrderByDescending(p => 
                            p.Position) 
                        : players.OrderBy(p => 
                            p.Position);
                }

                if(query.SortBy.Equals("TeamName", StringComparison.OrdinalIgnoreCase))
                {
                    players = query.IsDescending 
                        ? players.OrderByDescending(p => 
                            p.Team != null ? p.Team.Name : "") 
                        : players.OrderBy(p => 
                            p.Team != null ? p.Team.Name : "");
                }
            }

            return await players.Include(p => p.PlayerInjury).ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players.Include(p => p.PlayerInjury).FirstOrDefaultAsync(i => i.Id == id);
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