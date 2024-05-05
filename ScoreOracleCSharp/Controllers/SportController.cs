using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Mappers;

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

        // Get All Sports
        [HttpGet]
        public IActionResult GetAll() 
        {
            var sports = _context.Sports.ToList();
        
            return Ok(sports);
        }

        // Get Sport By ID
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

        // Create Sport
        [HttpPost]
        public IActionResult Create([FromBody] CreateSportDto sportDto)
        {
            if (string.IsNullOrWhiteSpace(sportDto.Name) || string.IsNullOrWhiteSpace(sportDto.Abbreviation))
            {
                return BadRequest("The sport name and abbreviation cannot be empty.");
            }

            if (SportExists(sportDto.Name, sportDto.Abbreviation))
            {
                return BadRequest("A sport with the same name or abbreviation already exists.");
            }

            var newSport = SportMapper.ToSportFromCreateDTO(sportDto);
            _context.Sports.Add(newSport);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newSport.Id }, SportMapper.ToSportDto(newSport));
        }
        
        private bool SportExists(string name, string abbreviation)
        {
            return _context.Sports.Any(s => s.Name == name || s.Abbreviation == abbreviation);
        }
    }
}