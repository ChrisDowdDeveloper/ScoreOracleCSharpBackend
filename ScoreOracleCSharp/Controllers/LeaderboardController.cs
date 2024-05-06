using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public LeaderboardController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all leaderboards in the database.
        /// </summary>
        /// <returns>A list of leaderboards</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var leaderboards = await _context.Leaderboards.ToListAsync();
        
            return Ok(leaderboards);
        }

        /// <summary>
        /// Retrieves a leaderboard in the database.
        /// </summary>
        /// <returns>A specific leaderboard</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(id);

            if(leaderboard == null)
            {
                return NotFound();
            }

            return Ok(leaderboard);
        }

        /// <summary>
        /// Creates a leaderboard in the database
        /// </summary>
        /// <returns>The created leaderboard</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaderboardDto leaderboardDto)
        {
            if(!await SportExists(leaderboardDto.SportId))
            {
                return BadRequest("Sport does not exist with that ID.");
            }

            var newLeaderboard = LeaderboardMapper.ToLeaderboardFromCreateDTO(leaderboardDto);
            _context.Leaderboards.Add(newLeaderboard);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newLeaderboard.Id }, LeaderboardMapper.ToLeaderboardDto(newLeaderboard));
        }

        /// <summary>
        /// Updates a leaderboard in the database
        /// </summary>
        /// <returns>The updated leaderboard</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLeaderboard([FromRoute] int id, [FromBody] UpdateLeaderboardDto updateDto)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(id);
            if (leaderboard == null)
            {
                return NotFound("Leaderboard not found.");
            }

            if (updateDto.Name != null)
            {
                leaderboard.Name = updateDto.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Type) && Enum.TryParse<LeaderboardType>(updateDto.Type, true, out var parsedType))
            {
                leaderboard.Type = parsedType;
            }
            else if (!string.IsNullOrWhiteSpace(updateDto.Type))
            {
                return BadRequest("Invalid leaderboard type specified.");
            }

            if (updateDto.SportId.HasValue && !await SportExists(updateDto.SportId.Value))
            {
                return BadRequest("Specified sport does not exist.");
            }
            else if (updateDto.SportId.HasValue)
            {
                leaderboard.SportId = updateDto.SportId.Value;
            }

            await _context.SaveChangesAsync();
            return Ok(LeaderboardMapper.ToLeaderboardDto(leaderboard));
        }

        /// <summary>
        /// Deletes a leaderboard in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var leaderboard = await _context.Leaderboards.FirstOrDefaultAsync(l => l.Id == id);
            if(leaderboard == null)
            {
                return NotFound("Leaderboard not found and could not be deleted");
            }

            _context.Leaderboards.Remove(leaderboard);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }
    }
}