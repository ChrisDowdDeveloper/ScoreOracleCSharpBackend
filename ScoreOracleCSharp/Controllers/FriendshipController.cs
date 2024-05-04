using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public FriendshipController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get All Friendships
        [HttpGet]
        public IActionResult GetAll()
        {
            var friendships = _context.Friendships.ToList();

            return Ok(friendships);
        }
        
        // Get Friendships by ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var friendship = _context.Friendships.Find(id);

            if(friendship == null)
            {
                return NotFound();
            }

            return Ok(friendship);
        }

        public IActionResult Create([FromBody] CreateFriendshipDto friendshipDto)
        {

            if(friendshipDto.RequesterId == friendshipDto.ReceiverId)
            {
                return BadRequest("Cannot create a friendship with oneself.");
            }

            var newFriendship = FriendshipMapper.ToFriendshipFromCreateDTO(friendshipDto);
            _context.Friendships.Add(newFriendship);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newFriendship.Id }, FriendshipMapper.ToFriendshipDto(newFriendship));

        }
    }
}