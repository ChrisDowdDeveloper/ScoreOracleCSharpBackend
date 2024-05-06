using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserScoreController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserScoreController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all user scores in the database.
        /// </summary>
        /// <returns>A list of user scores</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var scores = await _context.UserScores.ToListAsync();
        
            return Ok(scores);
        }

        /// <summary>
        /// Retrieves a user score in the database.
        /// </summary>
        /// <returns>A specific user score</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var score = await _context.UserScores.FindAsync(id);

            if(score == null)
            {
                return NotFound();
            }

            return Ok(score);
        }

        /// <summary>
        /// Creates a user score in the database
        /// </summary>
        /// <returns>The created user score</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserScoreDto userScoreDto)
        {
            if (userScoreDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if(!await UserExists(userScoreDto.UserId))
            {
                return BadRequest("User with that ID does not exists");
            }

            if(!await LeaderboardExists(userScoreDto.LeaderboardId))
            {
                return BadRequest("Leaderboard does not exist with that ID");
            }

            var newUserScore = UserScoreMapper.ToUserScoreFromCreateDTO(userScoreDto);
            _context.UserScores.Add(newUserScore);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newUserScore.Id }, UserScoreMapper.ToUserScoreDto(newUserScore));
        }

        /// <summary>
        /// Updates a user score in the database
        /// </summary>
        /// <returns>The updated user score</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserScoreDto userScoreDto)
        {
            var userScore = await _context.UserScores.FindAsync(id);
            if(userScore == null)
            {
                return NotFound("User Score cannot be found");
            }

            if (userScoreDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }

            if(userScoreDto.UserId.HasValue && !await UserExists(userScoreDto.UserId.Value))
            {
                return BadRequest("User does not exist");
            }
            else if(userScore.UserId.HasValue)
            {
                userScore.UserId = userScoreDto.UserId;
            }

            if(userScoreDto.LeaderboardId.HasValue && !await LeaderboardExists(userScoreDto.LeaderboardId.Value))
            {
                return BadRequest("Leaderboard does not exist.");
            }
            else if(userScoreDto.LeaderboardId.HasValue)
            {
                userScore.LeaderboardId = userScoreDto.LeaderboardId;
            }

            if (userScoreDto.Score != userScore.Score)
            {
                userScore.Score = userScoreDto.Score;
            }
            userScore.UpdatedLast = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(UserScoreMapper.ToUserScoreDto(userScore));

        }

        /// <summary>
        /// Deletes a user score in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (GetAuthenticatedUserId() != id)
            {
                return Unauthorized("Not authorized to update this friendship.");
            }

            var userScore = await _context.UserScores.FirstOrDefaultAsync(us => us.Id == id);
            if(userScore == null)
            {
                return NotFound("Users Score not found and could not be deleted");
            }

            _context.UserScores.Remove(userScore);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> UserExists(int userId) 
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
        private async Task<bool> LeaderboardExists(int leaderboardId)
        {
            return await _context.Leaderboards.AnyAsync(l => l.Id == leaderboardId);
        }
        private int GetAuthenticatedUserId()
        {
            return 0;
        }

    }
}