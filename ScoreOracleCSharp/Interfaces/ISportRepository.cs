using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface ISportRepository
    {
        Task<List<Sport>> GetAllAsync();
        Task<Sport?> GetByIdAsync(int id);
        Task<Sport> CreateAsync(Sport sportModel);
        Task<Sport?> UpdateAsync(int id, UpdateSportDto sportDto);
        Task<Sport?> DeleteAsync(int id);
        Task<bool> SportExists(string name, string abbreviation);
    }
}