using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        /// <summary>
        /// Retrieves all sports in the database.
        /// </summary>
        /// <returns>A list of sports</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var sports = await _context.Sports.ToListAsync();
        
            return Ok(sports);
        }

        /// <summary>
        /// Retrieves a sport in the database.
        /// </summary>
        /// <returns>A specific sport</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var sport = await _context.Sports.FindAsync(id);

            if(sport == null)
            {
                return NotFound();
            }

            return Ok(sport);
        }

        /// <summary>
        /// Creates a sport in the database
        /// </summary>
        /// <returns>The created sport</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSportDto sportDto)
        {
            if (string.IsNullOrWhiteSpace(sportDto.Name) || string.IsNullOrWhiteSpace(sportDto.Abbreviation))
            {
                return BadRequest("The sport name and abbreviation cannot be empty.");
            }

            if (await SportExists(sportDto.Name, sportDto.Abbreviation))
            {
                return BadRequest("A sport with the same name or abbreviation already exists.");
            }

            var newSport = SportMapper.ToSportFromCreateDTO(sportDto);
            _context.Sports.Add(newSport);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newSport.Id }, SportMapper.ToSportDto(newSport));
        }

        /// <summary>
        /// Updates a sport in the database
        /// </summary>
        /// <returns>The updated sport</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSportDto sportDto)
        {
            var sport = await _context.Sports.FindAsync(id);
            if (sport == null)
            {
                return NotFound("Sport cannot be found.");
            }

            if (sportDto.Name.Length < 2)
            {
                return BadRequest("Sport name must be longer than one character.");
            }

            if (sportDto.Abbreviation.Length < 3)
            {
                return BadRequest("Abbreviation must be at least three characters long.");
            }

            bool nameOrAbbreviationExists = await _context.Sports
                .AnyAsync(s => (s.Name == sportDto.Name || s.Abbreviation == sportDto.Abbreviation) && s.Id != id);
            if (nameOrAbbreviationExists)
            {
                return BadRequest("A sport with the same name or abbreviation already exists.");
            }

            sport.Name = sportDto.Name;
            sport.Abbreviation = sportDto.Abbreviation;
            sport.LogoURL = sportDto.LogoURL;
            sport.League = sportDto.League;

            await _context.SaveChangesAsync();
            return Ok(SportMapper.ToSportDto(sport));
        }

        /// <summary>
        /// Deletes a sport in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Id == id);
            if(sport == null)
            {
                return NotFound("Sport not found and could not be deleted");
            }

            _context.Sports.Remove(sport);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        private async Task<bool> SportExists(string name, string abbreviation)
        {
            return await _context.Sports.AnyAsync(s => s.Name == name || s.Abbreviation == abbreviation);
        }

    }
}