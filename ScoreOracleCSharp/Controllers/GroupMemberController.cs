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
        [HttpPost("{groupId}/members")]
        public async Task<IActionResult> AddMember(int groupId, [FromBody] CreateGroupMemberDto memberDto)
        { 

            if (!await UserHasPermissionToUpdate(groupId))
            {
                return Unauthorized("You do not have permission to add members to this group.");
            }

            if(!await GroupExists(memberDto.GroupId))
            {
                return BadRequest("Invalid group ID.");
            }

            var newGroupMember = GroupMemberMapper.ToGroupMemberFromCreateDTO(memberDto);
            _context.GroupMembers.Add(newGroupMember);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newGroupMember.Id }, GroupMemberMapper.ToGroupMemberDto(newGroupMember));
            
        }

        /// <summary>
        /// Deletes a group member in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete("{groupId}/members/{memberId}")]
        public async Task<IActionResult> RemoveMember(int groupId, int memberId)
        {
            if (!await UserHasPermissionToUpdate(groupId))
            {
                return Unauthorized("You do not have permission to remove members from this group.");
            }

            var member = await _context.Friendships.FirstOrDefaultAsync(m => m.Id == memberId);
            if(member == null)
            {
                return NotFound("Group member not found and could not be deleted");
            }

            _context.Friendships.Remove(member);
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

        private async Task<bool> GroupExists(int groupId)
        {
            return await _context.Groups.AnyAsync(g => g.Id == groupId);
        }

        private async Task<bool> UserHasPermissionToUpdate(int groupId)
        {
            var userId = GetAuthenticatedUserId(); // Method to get the logged-in user's ID
            var group = await _context.Groups.FindAsync(groupId);
            return group != null && group.CreatedByUserId == userId;
        }
    }
}