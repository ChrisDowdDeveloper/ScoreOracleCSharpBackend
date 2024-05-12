using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Team;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class TeamRepository : ITeamRepository
    {

        public ApplicationDBContext _context;
        public TeamRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Team> CreateAsync(Team teamModel)
        {
            await _context.Teams.AddAsync(teamModel);
            await _context.SaveChangesAsync();
            return teamModel;
        }

        public async Task<Team?> DeleteAsync(int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if(team == null)
            {
                return null;
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<List<Team>> GetAllAsync(TeamQueryObject query)
        {
            var teams = _context.Teams.Include(t => t.Sport).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.City))
            {
                teams = teams.Where(t => t.City.Contains(query.City));
            }

            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                teams = teams.Where(t => t.Name.Contains(query.Name));
            }

            if(!string.IsNullOrWhiteSpace(query.SportName))
            {
                teams = teams.Where(t => t.Sport != null && t.Sport.Name.Contains(query.SportName));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("City", StringComparison.OrdinalIgnoreCase))
                {
                    teams = query.IsDescending 
                        ? teams.OrderByDescending(t => 
                            t.City) 
                        : teams.OrderBy(t => 
                            t.City);
                }

                if(query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    teams = query.IsDescending 
                        ? teams.OrderByDescending(t => 
                            t.Name) 
                        : teams.OrderBy(t => 
                            t.Name);
                }

                if(query.SortBy.Equals("SportName", StringComparison.OrdinalIgnoreCase))
                {
                    teams = query.IsDescending 
                        ? teams.OrderByDescending(t => 
                            t.Sport != null ? t.Sport.Name : "") 
                        : teams.OrderBy(t => 
                            t.Sport != null ? t.Sport.Name : "");
                }
            }

            return await teams
                        .Include(h => h.HomeGames)
                        .Include(a => a.AwayGames)
                        .Include(i => i.InjuriesOnTeam)
                        .Include(p => p.PlayersOnTeam)
                        .Include(t => t.TeamPredicted)
                        .ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams
                                    .Include(h => h.HomeGames)
                                    .Include(a => a.AwayGames)
                                    .Include(i => i.InjuriesOnTeam)
                                    .Include(p => p.PlayersOnTeam)
                                    .Include(t => t.TeamPredicted)
                                    .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }

        public async Task<bool> TeamExists(string teamCity, string teamName, int sportId)
        {
            return await _context.Teams.AnyAsync(t => t.Name == teamName && t.City == teamCity && t.SportId == sportId);
        }

        public async Task<Team?> UpdateAsync(int id, UpdateTeamDto teamDto)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if(team == null)
            {
                return null;
            }

            if(teamDto.SportId.HasValue && !await SportExists(teamDto.SportId.Value))
            {
                throw new Exception("Sport is not valid.");
            }
            else if (teamDto.SportId.HasValue)
            {   
                team.SportId = teamDto.SportId.Value;
            }

            team.City = teamDto.City;
            team.Name = teamDto.Name;
            team.LogoURL = teamDto.LogoURL;

            await _context.SaveChangesAsync();
            return team;
        }

    }
}