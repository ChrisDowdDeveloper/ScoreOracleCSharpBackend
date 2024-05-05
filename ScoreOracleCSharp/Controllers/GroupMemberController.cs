using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Mappers;

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
    }
}