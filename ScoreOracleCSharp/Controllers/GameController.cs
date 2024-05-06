using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GameController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all games in the database.
        /// </summary>
        /// <returns>A list of games</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var games = await _context.Games.ToListAsync();
        
            return Ok(games);
        }

        /// <summary>
        /// Retrieves a game in the database.
        /// </summary>
        /// <returns>A specific game</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var game =await  _context.Games.FindAsync(id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        /// <summary>
        /// Creates a game in the database
        /// </summary>
        /// <returns>The created game</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGameDto gameDto)
        {
            if(gameDto.HomeTeamId == gameDto.AwayTeamId)
            {
                return BadRequest("The same team cannot play eachother.");
            }

            if(!await TeamExists(gameDto.HomeTeamId) || !await TeamExists(gameDto.AwayTeamId))
            {
                return BadRequest("One or both teams do not exist");
            }

            var newGame = GameMapper.ToGameFromCreateDTO(gameDto);
            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, GameMapper.ToGameDto(newGame));
        
        }

        /// <summary>
        /// Updates a game in the database
        /// </summary>
        /// <returns>The updated game</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGameDto gameDto)
        {
            var game = await _context.Games.FindAsync(id);
            if(game == null)
            {
                return NotFound();
            }

            if (gameDto.HomeTeamId.HasValue && !await TeamExists(gameDto.HomeTeamId.Value))
            {
                return BadRequest("Invalid Home team ID.");
            }

            if (gameDto.AwayTeamId.HasValue && !await TeamExists(gameDto.AwayTeamId.Value))
            {
                return BadRequest("Invalid Away team ID.");
            }

            if (gameDto.SportId.HasValue && !await SportExists(gameDto.SportId.Value))
            {
                return BadRequest("Invalid sport ID.");
            }

            if (gameDto.HomeTeamId.HasValue) game.HomeTeamId = gameDto.HomeTeamId;
            if (gameDto.AwayTeamId.HasValue) game.AwayTeamId = gameDto.AwayTeamId;
            if (gameDto.GameDate.HasValue) game.GameDate = gameDto.GameDate.Value;
            if (gameDto.GameStatus.HasValue) game.GameStatus = gameDto.GameStatus.Value;
            if (gameDto.SportId.HasValue) game.SportId = gameDto.SportId.Value;

            await _context.SaveChangesAsync();
            return Ok(GameMapper.ToGameDto(game));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if(game == null)
            {
                return NotFound("Game could not be found or deleted.");
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        private async Task<bool> SportExists(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }
    }
}