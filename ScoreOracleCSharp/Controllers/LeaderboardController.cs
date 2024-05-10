using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly ApplicationDBContext _context;
        public LeaderboardController(ApplicationDBContext context, ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all leaderboards in the database.
        /// </summary>
        /// <returns>A list of leaderboards</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(LeaderboardQueryObject query) 
        {
            var leaderboards = await _leaderboardRepository.GetAllAsync(query);
        
            return Ok(leaderboards);
        }

        /// <summary>
        /// Retrieves a leaderboard in the database.
        /// </summary>
        /// <returns>A specific leaderboard</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var leaderboard = await _leaderboardRepository.GetByIdAsync(id);

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
            if(!await _leaderboardRepository.SportExists(leaderboardDto.SportId))
            {
                return BadRequest("Sport does not exist with that ID.");
            }

            var newLeaderboard = LeaderboardMapper.ToLeaderboardFromCreateDTO(leaderboardDto);
            var createdLeaderboard = await _leaderboardRepository.CreateAsync(newLeaderboard);
            return CreatedAtAction(nameof(GetById), new { id = newLeaderboard.Id }, LeaderboardMapper.ToLeaderboardDto(createdLeaderboard));
        }

        /// <summary>
        /// Updates a leaderboard in the database
        /// </summary>
        /// <returns>The updated leaderboard</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateLeaderboard([FromRoute] int id, [FromBody] UpdateLeaderboardDto updateDto)
        {
            try
            {
                var updatedLeaderboard = await _leaderboardRepository.UpdateAsync(id, updateDto);
                if (updatedLeaderboard == null)
                {
                    return NotFound("Leaderboard not found.");
                }

                return Ok(LeaderboardMapper.ToLeaderboardDto(updatedLeaderboard));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Deletes a leaderboard in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _leaderboardRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}