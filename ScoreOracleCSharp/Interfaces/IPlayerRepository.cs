using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task<Player> CreateAsync(Player playerModel);
        Task<Player?> UpdateAsync(int id, UpdatePlayerDto playerDto);
        Task<Player?> DeleteAsync(int id);
        Task<bool> TeamExists(int teamId);
    }
}