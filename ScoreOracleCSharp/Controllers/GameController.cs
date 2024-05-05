using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}