using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult GetAll() 
        {
            var scores = _context.UserScores.ToList();
        
            return Ok(scores);
        }

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
    }
}