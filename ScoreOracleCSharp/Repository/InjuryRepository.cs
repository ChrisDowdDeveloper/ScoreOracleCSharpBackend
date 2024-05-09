using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class InjuryRepository : IInjuryRepository
    {
        private readonly ApplicationDBContext _context;
        public InjuryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Injury> CreateAsync(Injury injuryModel)
        {
            await _context.Injuries.AddAsync(injuryModel);
            await _context.SaveChangesAsync();
            return injuryModel;
        }

        public async Task<Injury?> DeleteAsync(int id)
        {
            var injury = await _context.Injuries.FirstOrDefaultAsync(i => i.Id == id);
            if(injury == null)
            {
                return null;
            }

            _context.Injuries.Remove(injury);
            await _context.SaveChangesAsync();
            return injury;
        }

        public async Task<List<Injury>> GetAllAsync()
        {
            return await _context.Injuries.ToListAsync();
        }

        public async Task<Injury?> GetByIdAsync(int id)
        {
            return await _context.Injuries.FindAsync(id);
        }

        public async Task<bool> PlayerExists(int id)
        {
            return await _context.Players.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> PlayerOnTeam(int playerId, int teamId)
        {
            return await _context.Players.AnyAsync(p => p.Id == playerId && p.TeamId == teamId);
        }

        public async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        public async Task<Injury?> UpdateAsync(int id, UpdateInjuryDto injuryDto)
        {
            var injury = await _context.Injuries.FirstOrDefaultAsync(i => i.Id == id);
            if(injury == null)
            {
                return null;
            }
            injury.PlayerId = injuryDto.PlayerId;
            injury.TeamId = injuryDto.TeamId;
            injury.Description = injuryDto.Description;

            await _context.SaveChangesAsync();
            return injury;
        }
    }
}