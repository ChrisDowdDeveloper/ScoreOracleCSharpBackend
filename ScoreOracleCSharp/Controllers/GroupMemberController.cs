using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupMemberController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GroupMemberController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all group members in the database.
        /// </summary>
        /// <returns>A list of group members</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var members = await _context.GroupMembers.ToListAsync();
        
            return Ok(members);
        }

        /// <summary>
        /// Retrieves a group member in the database.
        /// </summary>
        /// <returns>A specific group member</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var member = await _context.GroupMembers.FindAsync(id);

            if(member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        /// <summary>
        /// Creates a group member in the database
        /// </summary>
        /// <returns>The created group member</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupMemberDto groupMemberDto)
        {

            if(!await UserExists(groupMemberDto.UserId))
            {
                return BadRequest("Invalid user ID.");
            }

            if(groupMemberDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make this user a group member.");
            }

            if(!await GroupExists(groupMemberDto.GroupId))
            {
                return BadRequest("Invalid group ID.");
            }

            var newGroupMember = GroupMemberMapper.ToGroupMemberFromCreateDTO(groupMemberDto);
            _context.GroupMembers.Add(newGroupMember);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newGroupMember.Id }, GroupMemberMapper.ToGroupMemberDto(newGroupMember));
            
        }

        /// <summary>
        /// Updates a group member in the database
        /// </summary>
        /// <returns>The updated group member</returns>
        [HttpPatch]
        [Route("{memberId}")]
        public async Task<IActionResult> UpdateGroupMember([FromRoute] int memberId, [FromBody] UpdateGroupMemberDto groupMemberDto)
        {
            var member = await _context.GroupMembers.FindAsync(memberId);
            if(member == null)
            {
                return NotFound("Group member was not found.");
            }

            if(!await _context.Groups.AnyAsync(g => g.Id == groupMemberDto.GroupId))
            {
                return BadRequest("The group does not exist");
            }

            if(!UserHasPermissionToUpdate(memberId))
            {
                return Unauthorized("You do not have permission to update this group member.");
            }

            member.GroupId = groupMemberDto.GroupId;
            await _context.SaveChangesAsync();
            return Ok(GroupMemberMapper.ToGroupMemberDto(member));
        }

        /// <summary>
        /// Deletes a group member in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!UserHasPermissionToUpdate(id))
            {
                return Unauthorized("Not authorized to update this friendship.");
            }

            var member = await _context.Friendships.FirstOrDefaultAsync(m => m.Id == id);
            if(member == null)
            {
                return NotFound("Group member not found and could not be deleted");
            }

            _context.Friendships.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }

        private async Task<bool> GroupExists(int groupId)
        {
            return await _context.Groups.AnyAsync(g => g.Id == groupId);
        }

        private bool UserHasPermissionToUpdate(int memberId)
        {
            // Implement your authorization logic here
            // Example: Check if the current user is an admin or the group leader
            return true; // Placeholder
        }
    }
}