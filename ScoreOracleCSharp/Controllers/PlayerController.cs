using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PlayerController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all players in the database.
        /// </summary>
        /// <returns>A list of players</returns>
        [HttpGet]
        public IActionResult GetAll() 
        {
            var players = _context.Players.ToList();
        
            return Ok(players);
        }

        /// <summary>
        /// Retrieves a players in the database.
        /// </summary>
        /// <returns>A specific player</returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var player = _context.Players.Find(id);

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

            if(!await TeamExists(playerDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPlayer = PlayerMapper.ToPlayerFromCreateDTO(playerDto);
            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newPlayer.Id }, PlayerMapper.ToPlayerDto(newPlayer));
        }

        /// <summary>
        /// Updates a player in the database
        /// </summary>
        /// <returns>The updated player</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePlayerDto playerDto)
        {
            var player = await _context.Players.Include(p => p.PlayerInjury).FirstOrDefaultAsync(p => p.Id == id);
            if(player == null)
            {
                return NotFound();
            }

            if(playerDto.TeamId.HasValue && !await TeamExists(playerDto.TeamId.Value))
            {
                return BadRequest("Team does not exist");
            } 
            else if(playerDto.TeamId.HasValue)
            {
                player.TeamId = playerDto.TeamId.Value;
            }

            player.FirstName = playerDto.FirstName;
            player.LastName = playerDto.LastName;
            player.Position = playerDto.Position;

            if(playerDto.IsInjured && !player.PlayerInjury.Any())
            {
                player.PlayerInjury.Add(new Injury { 
                    PlayerId = player.Id,
                    Description = "This player is injured, please update accordingly",
                    TeamId = player.TeamId
                });
            }
            else if (!playerDto.IsInjured)
            {
                player.PlayerInjury.Clear();
            }

            await _context.SaveChangesAsync();
            return Ok(PlayerMapper.ToPlayerDto(player));
        }

        private async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }
    }
}