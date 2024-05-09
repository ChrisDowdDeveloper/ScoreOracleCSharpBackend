using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IGroupRepository _groupRepository;
        public GroupController(ApplicationDBContext context, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all groups in the database.
        /// </summary>
        /// <returns>A list of groups</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var groups = await _groupRepository.GetAllAsync();
        
            return Ok(groups);
        }

        /// <summary>
        /// Retrieves a group in the database.
        /// </summary>
        /// <returns>A specific group</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if(group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        /// <summary>
        /// Creates a group in the database
        /// </summary>
        /// <returns>The created group</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto groupDto)
        {
            if (!await _groupRepository.UserExists(groupDto.CreatedByUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            var userId = GetAuthenticatedUserId();
            if (userId == null)
            {
                return Unauthorized("You are not authorized to create a group.");
            }

            var newGroup = GroupMapper.ToGroupFromCreateDTO(groupDto);
            var createdGroup = await _groupRepository.CreateAsync(newGroup);
            return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, GroupMapper.ToGroupDto(createdGroup));
        }

        /// <summary>
        /// Updates a friendship in the database
        /// </summary>
        /// <returns>The updated friendship</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGroupDto groupDto)
        {
            var group = await _context.Groups.FindAsync(id);
            if(group == null)
            {
                return NotFound();
            }

            var userId = GetAuthenticatedUserId();
            if (!await _groupRepository.UserCanModifyGroup(userId, id))
            {
                return Unauthorized("You do not have permission to update this group.");
            }

            if (string.IsNullOrWhiteSpace(groupDto.Name) || groupDto.Name.Length < 3)
            {
                return BadRequest("Group name must have at least 3 characters and cannot be empty.");
            }

            var updatedGroup = await _groupRepository.UpdateAsync(id, groupDto);

            if(updatedGroup == null)
            {
                return NotFound("Group cannot be found.");
            }

            return Ok(GroupMapper.ToGroupDto(updatedGroup));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetAuthenticatedUserId();
            if(!await _groupRepository.UserCanModifyGroup(userId, id))
            {
                return Unauthorized("You do not have permission to delete this group");
            }

            await _groupRepository.DeleteAsync(id);
            return NoContent();
        }

        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }
        
    }
}