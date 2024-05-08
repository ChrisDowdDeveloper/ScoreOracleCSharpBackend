using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Team;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public TeamController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all teams in the database.
        /// </summary>
        /// <returns>A list of teams</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var teams = await _context.Teams.ToListAsync();
        
            return Ok(teams);
        }

        /// <summary>
        /// Retrieves a team in the database.
        /// </summary>
        /// <returns>A specific team</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if(team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        /// <summary>
        /// Creates a team in the database
        /// </summary>
        /// <returns>The created team</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto teamDto)
        {
            if(await TeamExists(teamDto.City, teamDto.Name, teamDto.SportId))
            {
                return BadRequest("Team in that city already exists with that name");
            }
            
            if(!await SportExists(teamDto.SportId))
            {
                return BadRequest("Sport with that ID does not exist");
            }

            var newTeam = TeamMapper.ToTeamFromCreateDTO(teamDto);
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newTeam.Id }, TeamMapper.ToTeamDto(newTeam));
        }

        /// <summary>
        /// Updates a team in the database
        /// </summary>
        /// <returns>The updated team</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTeamDto teamDto)
        {
            var team = await _context.Teams.FindAsync(id);
            if(team == null)
            {
                return NotFound("Team cannot be found.");
            }

            if(string.IsNullOrWhiteSpace(teamDto.City))
            {
                return BadRequest("Team must have a city.");
            }

            if(string.IsNullOrWhiteSpace(teamDto.Name))
            {
                return BadRequest("Team must have a name");
            }

            if(teamDto.SportId.HasValue && !await SportExists(teamDto.SportId.Value))
            {
                return BadRequest("Sport is not valid.");
            }
            else if (teamDto.SportId.HasValue)
            {   
                team.SportId = teamDto.SportId;
            }

            team.City = teamDto.City;
            team.Name = teamDto.Name;
            team.LogoURL = teamDto.LogoURL;

            await _context.SaveChangesAsync();
            return Ok(TeamMapper.ToTeamDto(team));

        }

        /// <summary>
        /// Deletes a team in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if(team == null)
            {
                return NotFound("Team not found and could not be deleted");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> TeamExists(string teamCity, string teamName, int sportId)
        {
            return await _context.Teams.AnyAsync(t => t.Name == teamName && t.City == teamCity && t.SportId == sportId);
        }

        private async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }
    }
}