using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class SportRepository : ISportRepository
    {
        private readonly ApplicationDBContext _context;
        public SportRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Sport> CreateAsync(Sport sportModel)
        {
            
            await _context.Sports.AddAsync(sportModel);
            await _context.SaveChangesAsync();
            return sportModel;
        }

        public async Task<Sport?> DeleteAsync(int id)
        {
            var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Id == id);
            if(sport == null)
            {
                return null;
            }
            _context.Sports.Remove(sport);
            await _context.SaveChangesAsync();
            return sport;
        }

        public async Task<List<Sport>> GetAllAsync(SportQueryObject query)
        {
            var sports = _context.Sports.AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                sports = sports.Where(s => s.Name.Contains(query.Name));
            }

            if(!string.IsNullOrWhiteSpace(query.League))
            {
                sports = sports.Where(s => s.League.Contains(query.League));
            }

            if(!string.IsNullOrWhiteSpace(query.Abbreviation))
            {
                sports = sports.Where(s => s.Abbreviation.Contains(query.Abbreviation));
            }

            return await sports.ToListAsync();
        }

        public async Task<Sport?> GetByIdAsync(int id)
        {
            return await _context.Sports.FindAsync(id);
        }

        public async Task<bool> SportExists(string name, string abbreviation)
        {
            return await _context.Sports.AnyAsync(s => s.Name == name || s.Abbreviation == abbreviation);
        }

        public async Task<Sport?> UpdateAsync(int id, UpdateSportDto sportDto)
        {
            var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Id == id);
            if(sport == null)
            {
                return null;
            }
            sport.Name = sportDto.Name;
            sport.Abbreviation = sportDto.Abbreviation;
            sport.LogoURL = sportDto.LogoURL;
            sport.League = sportDto.League;

            await _context.SaveChangesAsync();
            return sport;
        }
    }
}