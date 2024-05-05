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

        [HttpPost]
        public IActionResult Create([FromBody] CreateGroupDto groupDto)
        {
            if (!UserExists(groupDto.UserId))
            {
                return BadRequest("Invalid user ID.");
            }

            if (groupDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to create a group for another user.");
            }

            var newGroup = GroupMapper.ToGroupFromCreateDTO(groupDto);
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, GroupMapper.ToGroupDto(newGroup));
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