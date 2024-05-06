using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // Get All Groups
        [HttpGet]
        public IActionResult GetAll() 
        {
            var groups = _context.Groups.ToList();
        
            return Ok(groups);
        }

        // Get Groups By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var group = _context.Users.Find(id);

            if(group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // Create Group
        [HttpPost]
        public IActionResult Create([FromBody] CreateGroupDto groupDto)
        {
            if (!UserExists(groupDto.CreatedByUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            if (groupDto.CreatedByUserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to create a group for another user.");
            }

            var newGroup = GroupMapper.ToGroupFromCreateDTO(groupDto);
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, GroupMapper.ToGroupDto(newGroup));
        }

        // Update Group
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
            return Ok(new { message = "Group updated successfully", groupId = group.Id });
        }
        
        private bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }
        
    }
}