using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public SportController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var sports = _context.Sports.ToList();
        
            return Ok(sports);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var sport = _context.Sports.Find(id);

            if(sport == null)
            {
                return NotFound();
            }

            return Ok(sport);
        }
    }
}