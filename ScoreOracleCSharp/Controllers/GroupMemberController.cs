using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
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
        private readonly IGroupMemberRepository _memberRepository;
        public GroupMemberController(ApplicationDBContext context, IGroupMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all group members in the database.
        /// </summary>
        /// <returns>A list of group members</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(GroupMemberQueryObject query) 
        {
            var members = await _memberRepository.GetAllAsync(query);
        
            return Ok(members);
        }

        /// <summary>
        /// Retrieves a group member in the database.
        /// </summary>
        /// <returns>A specific group member</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

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
        [HttpPost("{groupId:int}/members")]
        public async Task<IActionResult> AddMember(int groupId, [FromBody] CreateGroupMemberDto memberDto)
        { 
            var userId = GetAuthenticatedUserId();

            if(!await _memberRepository.GroupExists(groupId))
            {
                return BadRequest("Invalid group ID.");
            }

            if (!await _memberRepository.UserCanModifyGroup(groupId, userId)) // Adjust the method to check permissions against groupId
            {
                return Unauthorized("You do not have permission to add members to this group.");
            }

            var newGroupMember = GroupMemberMapper.ToGroupMemberFromCreateDTO(memberDto);
            newGroupMember.GroupId = groupId; // Ensure the groupId is set correctly on the new member
            var createdGroupMember = await _memberRepository.CreateAsync(newGroupMember);
            return CreatedAtAction(nameof(GetById), new { id = newGroupMember.Id }, GroupMemberMapper.ToGroupMemberDto(createdGroupMember));
        }

        /// <summary>
        /// Deletes a group member in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete("{groupId:int}/members/{memberId:int}")]
        public async Task<IActionResult> RemoveMember(int groupId, int memberId)
        {
            var userId = GetAuthenticatedUserId();
            if (!await _memberRepository.UserCanModifyGroup(groupId, userId))
            {
                return Unauthorized("You do not have permission to remove members from this group.");
            }

            if(!await _memberRepository.GroupExists(groupId))
            {
                return BadRequest("Invalid group ID.");
            }

            await _memberRepository.DeleteAsync(memberId);
            return NoContent();
        }
        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }
    }
}