using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IInjuryRepository
    {
        Task<List<Injury>> GetAllAsync();
        Task<Injury?> GetByIdAsync(int id);
        Task<Injury> CreateAsync(Injury injuryModel);
        Task<Injury?> UpdateAsync(int id, UpdateInjuryDto injuryDto);
        Task<Injury?> DeleteAsync(int id);
        Task<bool> PlayerExists(int id);
        Task<bool> TeamExists(int teamId);
        Task<bool> PlayerOnTeam(int playerId, int teamId);
    }
}