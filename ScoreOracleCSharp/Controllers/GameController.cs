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

        // Get All Games
        [HttpGet]
        public IActionResult GetAll() 
        {
            var games = _context.Games.ToList();
        
            return Ok(games);
        }

        // Get Games By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var game = _context.Games.Find(id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // Create Game
        [HttpPost]
        public IActionResult Create([FromBody] CreateGameDto gameDto)
        {
            if(gameDto.HomeTeamId == gameDto.AwayTeamId)
            {
                return BadRequest("The same team cannot play eachother.");
            }

            var homeTeamExists = _context.Teams.Any(t => t.Id == gameDto.HomeTeamId);
            var awayTeamExists = _context.Teams.Any(t => t.Id == gameDto.AwayTeamId);
            if(!homeTeamExists || !awayTeamExists)
            {
                return BadRequest("One or both teams do not exist");
            }

            var newGame = GameMapper.ToGameFromCreateDTO(gameDto);
            _context.Games.Add(newGame);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, GameMapper.ToGameDto(newGame));
        
        }

        // Update Game
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

            _context.SaveChanges();
            return Ok(new { message = "Game updated successfully" });
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