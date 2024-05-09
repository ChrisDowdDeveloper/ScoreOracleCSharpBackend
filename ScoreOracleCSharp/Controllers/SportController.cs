using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Repository;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ISportRepository _sportRepository;
        public SportController(ApplicationDBContext context, ISportRepository sportRepository)
        {
            _context = context;
            _sportRepository = sportRepository;
        }

        /// <summary>
        /// Retrieves all sports in the database.
        /// </summary>
        /// <returns>A list of sports</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var sports = await _sportRepository.GetAllAsync();
        
            return Ok(sports);
        }

        /// <summary>
        /// Retrieves a sport in the database.
        /// </summary>
        /// <returns>A specific sport</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var sport = await _sportRepository.GetByIdAsync(id);

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

            if (await _sportRepository.SportExists(sportDto.Name, sportDto.Abbreviation))
            {
                return BadRequest("A sport with the same name or abbreviation already exists.");
            }

            var newSport = SportMapper.ToSportFromCreateDTO(sportDto);
            var createdSport = await _sportRepository.CreateAsync(newSport);
            return CreatedAtAction(nameof(GetById), new { id = createdSport.Id }, SportMapper.ToSportDto(createdSport));
        }

        /// <summary>
        /// Updates a sport in the database
        /// </summary>
        /// <returns>The updated sport</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSportDto sportDto)
        {
            if (!await _sportRepository.SportExists(sportDto.Name, sportDto.Abbreviation))
            {
                return BadRequest("A sport with the same name or abbreviation already exists.");
            }

            var updatedSport = await _sportRepository.UpdateAsync(id, sportDto);
            if (updatedSport == null)
            {
                return NotFound("Sport cannot be found.");
            }

            return Ok(SportMapper.ToSportDto(updatedSport));
        }

        /// <summary>
        /// Deletes a sport in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var sport = await _sportRepository.DeleteAsync(id);
            if(sport == null)
            {
                return NotFound("Sport not found and could not be deleted");
            }

            return NoContent();
        }

    }
}