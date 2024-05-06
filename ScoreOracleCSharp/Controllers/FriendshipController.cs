using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Retrieves all friendships in the database.
        /// </summary>
        /// <returns>A list of friendships</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var friendships = await _context.Friendships.ToListAsync();

            return Ok(friendships);
        }
        
        /// <summary>
        /// Retrieves a friendship in the database.
        /// </summary>
        /// <returns>A specific friendship</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var friendship = await _context.Friendships.FindAsync(id);

            if(friendship == null)
            {
                return NotFound();
            }

            return Ok(friendship);
        }

        /// <summary>
        /// Creates a friendship in the database
        /// </summary>
        /// <returns>The created friendship</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFriendshipDto friendshipDto)
        {

            if(friendshipDto.RequesterId == friendshipDto.ReceiverId)
            {
                return BadRequest("Cannot create a friendship with oneself.");
            }

            var newFriendship = FriendshipMapper.ToFriendshipFromCreateDTO(friendshipDto);
            _context.Friendships.Add(newFriendship);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newFriendship.Id }, FriendshipMapper.ToFriendshipDto(newFriendship));

        }

        /// <summary>
        /// Updates a friendship in the database
        /// </summary>
        /// <returns>The updated friendship</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFriendshipDto friendshipDto)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if(friendship == null)
            {
                return NotFound();
            }

            if (!await UserExists(friendshipDto.RequesterId) || !await UserExists(friendshipDto.ReceiverId))
            {
                return BadRequest("Invalid requester or receiver ID.");
            }

            if (!IsAuthorizedToUpdateFriendship(id))
            {
                return Unauthorized("Not authorized to update this friendship.");
            }

            friendship.RequesterId = friendshipDto.RequesterId;
            friendship.ReceiverId = friendshipDto.ReceiverId;
            friendship.Status = friendshipDto.Status;
            

            await _context.SaveChangesAsync();
            return Ok(FriendshipMapper.ToFriendshipDto(friendship));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!IsAuthorizedToUpdateFriendship(id))
            {
                return Unauthorized("Not authorized to update this friendship.");
            }

            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == id);
            if(friendship == null)
            {
                return NotFound("Friendship not found and could not be deleted");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        private bool IsAuthorizedToUpdateFriendship(int friendshipId)
        {
            return true;
        }

    }
}