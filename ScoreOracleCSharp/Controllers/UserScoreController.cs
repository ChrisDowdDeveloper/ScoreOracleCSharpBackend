using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserScoreController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserScoreRepository _scoreRepository;
        public UserScoreController(ApplicationDBContext context, IUserScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserScoreQueryObject query) 
        {
            var score = await _scoreRepository.GetAllAsync(query);
            return Ok(score);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var scores = await _scoreRepository.GetByIdAsync(id);
            if (scores == null)
            {
                return NotFound();
            }
            return Ok(scores);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserScoreDto userScoreDto)
        {
            var userId = GetAuthenticatedUserId();
            if (userScoreDto.UserId != userId)
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if (!await _scoreRepository.UserExists(userId))
            {
                return BadRequest("User does not exist.");
            }

            if (!await _scoreRepository.LeaderboardExists(userScoreDto.LeaderboardId))
            {
                return BadRequest("Leaderboard does not exist.");
            }

            var newUserScore = UserScoreMapper.ToUserScoreFromCreateDTO(userScoreDto);
            var createdUserScore = await _scoreRepository.CreateAsync(newUserScore);
            return CreatedAtAction(nameof(GetById), new { id = newUserScore.Id }, UserScoreMapper.ToUserScoreDto(createdUserScore));
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserScoreDto userScoreDto)
        {
            var userId = GetAuthenticatedUserId();
            if(userScoreDto.UserId == userId)
            {
                return Unauthorized("You are not authorized to update this score.");
            }
            var updatedUserScore = await _scoreRepository.UpdateAsync(id, userScoreDto);
            if(updatedUserScore == null)
            {
                return NotFound("User Scores cannot be found");
            }
            return Ok(UserScoreMapper.ToUserScoreDto(updatedUserScore));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _scoreRepository.DeleteAsync(id);
            return NoContent();
        }
        private string GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (userId == null)
            {
                throw new InvalidOperationException("User must be authenticated.");
            }
            else
            {
                return userId;
            }
        }
    }
}
