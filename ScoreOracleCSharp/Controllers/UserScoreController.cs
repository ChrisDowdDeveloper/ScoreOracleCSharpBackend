using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create([FromBody] CreateUserScoreDto userScoreDto)
        {
            if (userScoreDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if(!UserExists(userScoreDto.UserId))
            {
                return BadRequest("User with that ID does not exists");
            }

            if(!LeaderboardExists(userScoreDto.LeaderboardId))
            {
                return BadRequest("Leaderboard does not exist with that ID");
            }

            var newUserScore = UserScoreMapper.ToUserScoreFromCreateDTO(userScoreDto);
            _context.UserScores.Add(newUserScore);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newUserScore.Id }, UserScoreMapper.ToUserScoreDto(newUserScore));
        }

        public bool UserExists(int userId) 
        {
            return _context.Users.Any(u => u.Id == userId);
        }
        public bool LeaderboardExists(int leaderboardId)
        {
            return _context.Leaderboards.Any(l => l.Id == leaderboardId);
        }
        private int GetAuthenticatedUserId()
        {
            return 0;
        }

    }
}