using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InjuryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IInjuryRepository _injuryRepository;
        public InjuryController(ApplicationDBContext context, IInjuryRepository injuryRepository)
        {
            _injuryRepository = injuryRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all injuries in the database.
        /// </summary>
        /// <returns>A list of injuries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var injuries = await _injuryRepository.GetAllAsync();
        
            return Ok(injuries);
        }

        /// <summary>
        /// Retrieves an injury in the database.
        /// </summary>
        /// <returns>A specific injury</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var injury = await _injuryRepository.GetByIdAsync(id);

            if(injury == null)
            {
                return NotFound();
            }

            return Ok(injury);
        }

        /// <summary>
        /// Creates an injury in the database
        /// </summary>
        /// <returns>The created injury</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInjuryDto injuryDto)
        {
            if(!await _injuryRepository.PlayerExists(injuryDto.PlayerId))
            {
                return BadRequest("Player does not exist with that ID.");
            }

            if(!await _injuryRepository.TeamExists(injuryDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID.");
            }

            if(!await _injuryRepository.PlayerOnTeam(injuryDto.PlayerId, injuryDto.TeamId))
            {
                return BadRequest("Player is not on that team");
            }

            var newInjury = InjuryMapper.ToInjuryFromCreateDTO(injuryDto);
            var createdInjury = await _injuryRepository.CreateAsync(newInjury);
            return CreatedAtAction(nameof(GetById), new { id = newInjury.Id }, InjuryMapper.ToInjuryDto(createdInjury));
        }

        /// <summary>
        /// Updates an injury in the database
        /// </summary>
        /// <returns>The updated injury</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInjuryDto injuryDto)
        {
            var updatedInjury = await _injuryRepository.UpdateAsync(id, injuryDto);
            if(updatedInjury == null)
            {
                return NotFound("Injury cannot be found or deleted");
            }
            return Ok(InjuryMapper.ToInjuryDto(updatedInjury));
        }

        /// <summary>
        /// Deletes an injury in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _injuryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}