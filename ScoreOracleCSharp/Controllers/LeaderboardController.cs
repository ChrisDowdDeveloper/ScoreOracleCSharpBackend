using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public LeaderboardController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get All Leaderboards
        [HttpGet]
        public IActionResult GetAll() 
        {
            var leaderboards = _context.Leaderboards.ToList();
        
            return Ok(leaderboards);
        }

        // Get Leaderboard By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var leaderboard = _context.Leaderboards.Find(id);

            if(leaderboard == null)
            {
                return NotFound();
            }

            return Ok(leaderboard);
        }

        // Create Leaderboard
        public IActionResult Create([FromBody] CreateLeaderboardDto leaderboardDto)
        {
            if(!SportExists(leaderboardDto.SportId))
            {
                return BadRequest("Sport does not exist with that ID.");
            }

            var newLeaderboard = LeaderboardMapper.ToLeaderboardFromCreateDTO(leaderboardDto);
            _context.Leaderboards.Add(newLeaderboard);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newLeaderboard.Id }, LeaderboardMapper.ToLeaderboardDto(newLeaderboard));
        }

        private bool SportExists(int sportId)
        {
            return _context.Sports.Any(s => s.Id == sportId);
        }
    }
}