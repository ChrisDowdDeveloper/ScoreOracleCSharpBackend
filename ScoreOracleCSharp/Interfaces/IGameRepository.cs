using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync(GameQueryObject query);
        Task<Game?> GetByIdAsync(int id);
        Task<Game> CreateAsync(Game gameModel);
        Task<Game?> UpdateAsync(int id, UpdateGameDto gameDto);
        Task<Game?> DeleteAsync(int id);
        Task<bool> TeamExists(int teamId);
        Task<bool> SportExists(int sportId);
    }
}