using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PredictionController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var predictions = _context.Predictions.ToList();
        
            return Ok(predictions);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var prediction = _context.Predictions.Find(id);

            if(prediction == null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }
    }
}