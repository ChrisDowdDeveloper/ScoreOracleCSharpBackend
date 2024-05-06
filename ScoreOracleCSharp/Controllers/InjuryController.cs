using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InjuryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public InjuryController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get All Injuries
        [HttpGet]
        public IActionResult GetAll() 
        {
            var injuries = _context.Injuries.ToList();
        
            return Ok(injuries);
        }

        // Get Injury By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var injury = _context.Injuries.Find(id);

            if(injury == null)
            {
                return NotFound();
            }

            return Ok(injury);
        }

        // Create Injury 
        [HttpPost]
        public IActionResult Create([FromBody] CreateInjuryDto injuryDto)
        {
            if(!PlayerExists(injuryDto.PlayerId))
            {
                return BadRequest("Player does not exist with that ID.");
            }

            if(!TeamExists(injuryDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID.");
            }

            if(!PlayerOnTeam(injuryDto.PlayerId, injuryDto.TeamId))
            {
                return BadRequest("Player is not on that team");
            }

            var newInjury = InjuryMapper.ToInjuryFromCreateDTO(injuryDto);
            _context.Injuries.Add(newInjury);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newInjury.Id }, InjuryMapper.ToInjuryDto(newInjury));
        }

        // Update Injury
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInjuryDto injuryDto)
        {
            var injury = await _context.Injuries.FindAsync(id);
            if (injury == null)
            {
                return NotFound("Injury not found.");
            }

            if (injuryDto.PlayerId.HasValue && !await _context.Players.AnyAsync(p => p.Id == injuryDto.PlayerId.Value))
            {
                return BadRequest("Player does not exist.");
            }

            if (injuryDto.TeamId.HasValue && !await _context.Teams.AnyAsync(t => t.Id == injuryDto.TeamId.Value))
            {
                return BadRequest("The team does not exist.");
            }

            if (injuryDto.PlayerId.HasValue)
            {
                injury.PlayerId = injuryDto.PlayerId.Value;
            }

            if (injuryDto.TeamId.HasValue)
            {
                injury.TeamId = injuryDto.TeamId.Value;
            }

            injury.Description = injuryDto.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Injury has been updated successfully." });
        }
        private bool PlayerExists(int playerId)
        {
            return _context.Players.Any(p => p.Id == playerId);
        }

        private bool TeamExists(int teamId)
        {
            return _context.Teams.Any(t => t.Id == teamId);
        }

        private bool PlayerOnTeam(int playerId, int teamId)
        {
            return _context.Players.Any(p => p.Id == playerId && p.TeamId == teamId);
        }
    }
}