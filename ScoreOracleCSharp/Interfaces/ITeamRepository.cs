using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Team;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task<Team> CreateAsync(Team teamModel);
        Task<Team?> UpdateAsync(int id, UpdateTeamDto teamDto);
        Task<Team?> DeleteAsync(int id);
        Task<bool> SportExists(int sportId);
        Task<bool> TeamExists(string teamCity, string teamName, int sportId);
    }
}