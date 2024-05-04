using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InjuryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public InjuryController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var injuries = _context.Injuries.ToList();
        
            return Ok(injuries);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var injury = _context.Injuries.Find(id);

            if(injury == null)
            {
                return NotFound();
            }

            return Ok(injury);
        }
    }
}