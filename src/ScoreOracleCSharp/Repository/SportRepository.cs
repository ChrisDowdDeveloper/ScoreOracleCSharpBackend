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

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    sports = query.IsDescending 
                        ? sports.OrderByDescending(s => 
                            s.Name) 
                        : sports.OrderBy(s => 
                            s.Name);
                }

                if(query.SortBy.Equals("League", StringComparison.OrdinalIgnoreCase))
                {
                    sports = query.IsDescending 
                        ? sports.OrderByDescending(s => 
                            s.League) 
                        : sports.OrderBy(s => 
                            s.League);
                }

                if(query.SortBy.Equals("Abbreviation", StringComparison.OrdinalIgnoreCase))
                {
                    sports = query.IsDescending 
                        ? sports.OrderByDescending(s => 
                            s.Abbreviation) 
                        : sports.OrderBy(s => 
                            s.Abbreviation);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await sports
                        .Include(t => t.Teams)
                        .Include(g => g.Games)
                        .Include(p => p.PlayersInSport)
                        .Include(l => l.LeaderboardsBySport)
                        .Skip(skipNumber)
                        .Take(query.PageSize)
                        .ToListAsync();
        }

        public async Task<Sport?> GetByIdAsync(int id)
        {
            return await _context.Sports
                                 .Include(t => t.Teams)
                                 .Include(g => g.Games)
                                 .Include(p => p.PlayersInSport)
                                 .Include(l => l.LeaderboardsBySport)
                                 .FirstOrDefaultAsync(i => i.Id == id);
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