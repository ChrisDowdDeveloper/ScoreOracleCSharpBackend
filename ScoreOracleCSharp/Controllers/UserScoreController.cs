using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using System;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Ensure the controller actions are protected
    public class UserScoreController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserScoreController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var scores = await _context.UserScores.ToListAsync();
            var scoreDtos = scores.Select(score => UserScoreMapper.ToUserScoreDto(score)).ToList();
            return Ok(scoreDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var score = await _context.UserScores.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }
            return Ok(UserScoreMapper.ToUserScoreDto(score));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserScoreDto userScoreDto)
        {
            var userId = GetAuthenticatedUserId();
            if (userScoreDto.UserId != userId)
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if (!await UserExists(userId))
            {
                return BadRequest("User does not exist.");
            }

            if (!await LeaderboardExists(userScoreDto.LeaderboardId))
            {
                return BadRequest("Leaderboard does not exist.");
            }

            var newUserScore = UserScoreMapper.ToUserScoreFromCreateDTO(userScoreDto);
            _context.UserScores.Add(newUserScore);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newUserScore.Id }, UserScoreMapper.ToUserScoreDto(newUserScore));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserScoreDto userScoreDto)
        {
            var userScore = await _context.UserScores.FindAsync(id);
            if (userScore == null)
            {
                return NotFound();
            }

            var userId = GetAuthenticatedUserId();
            if (userScore.UserId != userId)
            {
                return Unauthorized("You are not authorized to update this score.");
            }

            userScore.Score = userScoreDto.Score;
            userScore.UpdatedLast = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(UserScoreMapper.ToUserScoreDto(userScore));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetAuthenticatedUserId();
            var userScore = await _context.UserScores.FirstOrDefaultAsync(us => us.Id == id && us.UserId == userId);
            if (userScore == null)
            {
                return NotFound("User score not found.");
            }

            _context.UserScores.Remove(userScore);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> UserExists(string userId) 
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
        private async Task<bool> LeaderboardExists(int leaderboardId)
        {
            return await _context.Leaderboards.AnyAsync(l => l.Id == leaderboardId);
        }
        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }
    }
}
