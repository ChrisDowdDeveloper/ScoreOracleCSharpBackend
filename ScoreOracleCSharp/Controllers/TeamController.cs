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

        // Get All Teams
        [HttpGet]
        public IActionResult GetAll() 
        {
            var teams = _context.Teams.ToList();
        
            return Ok(teams);
        }

        // Get Team By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var team = _context.Teams.Find(id);

            if(team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // Create Team
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto teamDto)
        {
            if(!await TeamExists(teamDto.City, teamDto.Name, teamDto.SportId))
            {
                return BadRequest("Team in that city already exists with that name");
            }
            
            if(!await SportExists(teamDto.SportId))
            {
                return BadRequest("Sport with that ID does not exist");
            }

            var newTeam = TeamMapper.ToTeamFromCreateDTO(teamDto);
            _context.Teams.Add(newTeam);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newTeam.Id }, TeamMapper.ToTeamDto(newTeam));
        }

        // Update Team
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