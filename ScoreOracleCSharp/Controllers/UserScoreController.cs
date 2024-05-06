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

        // Get All User Scores
        [HttpGet]
        public IActionResult GetAll() 
        {
            var scores = _context.UserScores.ToList();
        
            return Ok(scores);
        }

        // Get User Score By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var score = _context.UserScores.Find(id);

            if(score == null)
            {
                return NotFound();
            }

            return Ok(score);
        }

        // Create User Score
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
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newUserScore.Id }, UserScoreMapper.ToUserScoreDto(newUserScore));
        }

        // Update a User Score
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