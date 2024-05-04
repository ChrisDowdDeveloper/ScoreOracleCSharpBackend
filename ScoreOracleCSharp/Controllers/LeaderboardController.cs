using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult GetAll() 
        {
            var leaderboards = _context.Leaderboards.ToList();
        
            return Ok(leaderboards);
        }

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
    }
}