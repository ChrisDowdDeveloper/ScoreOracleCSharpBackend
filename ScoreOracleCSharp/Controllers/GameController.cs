using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IGameRepository _gameRepository;
        public GameController(ApplicationDBContext context, IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all games in the database.
        /// </summary>
        /// <returns>A list of games</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var games = await _gameRepository.GetAllAsync();

            return Ok(games);
        }

        /// <summary>
        /// Retrieves a game in the database.
        /// </summary>
        /// <returns>A specific game</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var game =await  _gameRepository.GetByIdAsync(id);

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

            if(!await _gameRepository.TeamExists(gameDto.HomeTeamId) || !await _gameRepository.TeamExists(gameDto.AwayTeamId))
            {
                return BadRequest("One or both teams do not exist");
            }

            var newGame = GameMapper.ToGameFromCreateDTO(gameDto);
            var createdGame = await _gameRepository.CreateAsync(newGame);
            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, GameMapper.ToGameDto(createdGame));
        
        }

        /// <summary>
        /// Updates a game in the database
        /// </summary>
        /// <returns>The updated game</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGameDto gameDto)
        {
            if (!gameDto.HomeTeamId.HasValue || !gameDto.AwayTeamId.HasValue)
            {
                return BadRequest("One or both team IDs are missing.");
            }

            if (!gameDto.SportId.HasValue)
            {
                return BadRequest("Sport ID is missing.");
            }

            if (!await _gameRepository.TeamExists(gameDto.HomeTeamId.Value) || !await _gameRepository.TeamExists(gameDto.AwayTeamId.Value))
            {
                return BadRequest("One or both teams do not exist.");
            }

            if (!await _gameRepository.SportExists(gameDto.SportId.Value))
            {
                return BadRequest("Invalid sport ID.");
            }

            var updatedGame = await _gameRepository.UpdateAsync(id, gameDto);

            if (updatedGame == null)
            {
                return NotFound("Game cannot be found");
            }

            return Ok(GameMapper.ToGameDto(updatedGame));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if(game == null)
            {
                return NotFound("Game cannot be found or deleted");
            }
            await _gameRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}