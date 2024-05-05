using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create([FromBody] CreateTeamDto teamDto)
        {
            if(!TeamExists(teamDto.City, teamDto.Name, teamDto.SportId))
            {
                return BadRequest("Team in that city already exists with that name");
            }
            
            if(!SportExists(teamDto.SportId))
            {
                return BadRequest("Sport with that ID does not exist");
            }

            var newTeam = TeamMapper.ToTeamFromCreateDTO(teamDto);
            _context.Teams.Add(newTeam);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newTeam.Id }, TeamMapper.ToTeamDto(newTeam));
        }

        public bool TeamExists(string teamCity, string teamName, int sportId)
        {
            return _context.Teams.Any(t => t.Name == teamName && t.City == teamCity && t.SportId == sportId);
        }

        public bool SportExists(int sportId)
        {
            return _context.Sports.Any(s => s.Id == sportId);
        }
    }
}