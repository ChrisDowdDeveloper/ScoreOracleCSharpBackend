using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GroupController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all groups in the database.
        /// </summary>
        /// <returns>A list of groups</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var groups = await _context.Groups.ToListAsync();
        
            return Ok(groups);
        }

        /// <summary>
        /// Retrieves a group in the database.
        /// </summary>
        /// <returns>A specific group</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var group = await _context.Users.FindAsync(id);

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
            if (!await UserExists(groupDto.CreatedByUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            if (groupDto.CreatedByUserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to create a group for another user.");
            }

            var newGroup = GroupMapper.ToGroupFromCreateDTO(groupDto);
            _context.Groups.Add(newGroup);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, GroupMapper.ToGroupDto(newGroup));
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
            if (group.CreatedByUserId != userId)
            {
                return Unauthorized("You do not have permission to update this group.");
            }

            if (string.IsNullOrWhiteSpace(groupDto.Name) || groupDto.Name.Length < 3)
            {
                return BadRequest("Group name must have at least 3 characters and cannot be empty.");
            }

            group.Name = groupDto.Name.Trim(); 

            group.Name = groupDto.Name;
            await _context.SaveChangesAsync();
            return Ok(GroupMapper.ToGroupDto(group));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if(group == null)
            {
                return NotFound("Group could not be found or deleted.");
            }

            var userId = GetAuthenticatedUserId();
            if (group.CreatedByUserId != userId)
            {
                return Unauthorized("You do not have permission to delete this group.");
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return NoContent();
        }        
        private async Task<bool> UserExists(string userId) 
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }
        
    }
}