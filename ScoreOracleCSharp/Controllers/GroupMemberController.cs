using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

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

        // Get All Group Members
        [HttpGet]
        public IActionResult GetAll() 
        {
            var members = _context.GroupMembers.ToList();
        
            return Ok(members);
        }

        // Get Group Member by ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var member = _context.GroupMembers.Find(id);

            if(member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // Create Group Member
        [HttpPost]
        public IActionResult Create([FromBody] CreateGroupMemberDto groupMemberDto)
        {
            if(!UserExists(groupMemberDto.UserId))
            {
                return BadRequest("Invalid user ID.");
            }

            if(groupMemberDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make this user a group member.");
            }

            if(!GroupExists(groupMemberDto.GroupId))
            {
                return BadRequest("Invalid group ID.");
            }

            var newGroupMember = GroupMemberMapper.ToGroupMemberFromCreateDTO(groupMemberDto);
            _context.GroupMembers.Add(newGroupMember);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newGroupMember.Id }, GroupMemberMapper.ToGroupMemberDto(newGroupMember));
            
        }

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

            if(!UserHasPermissionToUpdate(member))
            {
                return Unauthorized("You do not have permission to update this group member.");
            }

            member.GroupId = groupMemberDto.GroupId;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Group member updated successfully." });
        }

        private bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }

        private bool GroupExists(int groupId)
        {
            return _context.Groups.Any(g => g.Id == groupId);
        }

        private bool UserHasPermissionToUpdate(GroupMember member)
        {
            // Implement your authorization logic here
            // Example: Check if the current user is an admin or the group leader
            return true; // Placeholder
        }
    }
}