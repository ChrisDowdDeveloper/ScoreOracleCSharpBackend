using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Team;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ITeamRepository _teamRepository;
        public TeamController(ApplicationDBContext context, ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all teams in the database.
        /// </summary>
        /// <returns>A list of teams</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TeamQueryObject query) 
        {
            var teams = await _teamRepository.GetAllAsync(query);
        
            return Ok(teams);
        }

        /// <summary>
        /// Retrieves a team in the database.
        /// </summary>
        /// <returns>A specific team</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            if(team == null)
            {
                return NotFound();
            }

            return Ok(TeamMapper.ToTeamDto(team));
        }

        /// <summary>
        /// Creates a team in the database
        /// </summary>
        /// <returns>The created team</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto teamDto)
        {
            if(await _teamRepository.TeamExists(teamDto.City, teamDto.Name, teamDto.SportId))
            {
                return BadRequest("Team in that city already exists with that name");
            }
            
            if(!await _teamRepository.SportExists(teamDto.SportId))
            {
                return BadRequest("Sport with that ID does not exist");
            }

            var newTeam = TeamMapper.ToTeamFromCreateDTO(teamDto);
            var createdTeam = await _teamRepository.CreateAsync(newTeam);
            return CreatedAtAction(nameof(GetById), new { id = newTeam.Id }, TeamMapper.ToTeamDto(createdTeam));
        }

        /// <summary>
        /// Updates a team in the database
        /// </summary>
        /// <returns>The updated team</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTeamDto teamDto)
        {
            var updatedTeam = await _teamRepository.UpdateAsync(id, teamDto);
            if(updatedTeam == null)
            {
                return BadRequest("Team cannot be found.");
            }
            return Ok(TeamMapper.ToTeamDto(updatedTeam));

        }

        /// <summary>
        /// Deletes a team in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _teamRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}