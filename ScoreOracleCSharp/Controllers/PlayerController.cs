using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IPlayerRepository _playerRepository;
        public PlayerController(ApplicationDBContext context, IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all players in the database.
        /// </summary>
        /// <returns>A list of players</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(PlayerQueryObject query) 
        {
            var players = await _playerRepository.GetAllAsync(query);
        
            return Ok(players);
        }

        /// <summary>
        /// Retrieves a player in the database.
        /// </summary>
        /// <returns>A specific player</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            if(player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        /// <summary>
        /// Creates a player in the database
        /// </summary>
        /// <returns>The created player</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerDto playerDto)
        {

            if (string.IsNullOrWhiteSpace(playerDto.FirstName) || string.IsNullOrWhiteSpace(playerDto.LastName))
            {
                return BadRequest("Player name cannot be empty.");
            }

            if(!await _playerRepository.TeamExists(playerDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPlayer = PlayerMapper.ToPlayerFromCreateDTO(playerDto);
            var createdPlayer = await _playerRepository.CreateAsync(newPlayer);
            return CreatedAtAction(nameof(GetById), new { id = newPlayer.Id }, PlayerMapper.ToPlayerDto(createdPlayer));
        }

        /// <summary>
        /// Updates a player in the database
        /// </summary>
        /// <returns>The updated player</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePlayerDto playerDto)
        {
            try
            {
                var updatedPlayer = await _playerRepository.UpdateAsync(id, playerDto);
                if (updatedPlayer == null)
                {
                    return NotFound("Player not found.");
                }

                return Ok(PlayerMapper.ToPlayerDto(updatedPlayer));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a player in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _playerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}