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

        /// <summary>
        /// Retrieves all injuries in the database.
        /// </summary>
        /// <returns>A list of injuries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var injuries = await _context.Injuries.ToListAsync();
        
            return Ok(injuries);
        }

        /// <summary>
        /// Retrieves an injury in the database.
        /// </summary>
        /// <returns>A specific injury</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var injury = await _context.Injuries.FindAsync(id);

            if(injury == null)
            {
                return NotFound();
            }

            return Ok(injury);
        }

        /// <summary>
        /// Creates an injury in the database
        /// </summary>
        /// <returns>The created injury</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInjuryDto injuryDto)
        {
            if(!await PlayerExists(injuryDto.PlayerId))
            {
                return BadRequest("Player does not exist with that ID.");
            }

            if(!await TeamExists(injuryDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID.");
            }

            if(!await PlayerOnTeam(injuryDto.PlayerId, injuryDto.TeamId))
            {
                return BadRequest("Player is not on that team");
            }

            var newInjury = InjuryMapper.ToInjuryFromCreateDTO(injuryDto);
            _context.Injuries.Add(newInjury);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newInjury.Id }, InjuryMapper.ToInjuryDto(newInjury));
        }

        /// <summary>
        /// Updates an injury in the database
        /// </summary>
        /// <returns>The updated injury</returns>
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
            return Ok(InjuryMapper.ToInjuryDto(injury));
        }

        /// <summary>
        /// Deletes an injury in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var injury = await _context.Injuries.FirstOrDefaultAsync(i => i.Id == id);
            if(injury == null)
            {
                return NotFound("Injury not found and could not be deleted");
            }

            _context.Injuries.Remove(injury);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        private async Task<bool> PlayerExists(int playerId)
        {
            return await _context.Players.AnyAsync(p => p.Id == playerId);
        }

        private async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        private async Task<bool> PlayerOnTeam(int playerId, int teamId)
        {
            return await _context.Players.AnyAsync(p => p.Id == playerId && p.TeamId == teamId);
        }
    }
}