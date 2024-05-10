using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using ScoreOracleCSharp.Repository;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IFriendshipRepository _friendshipRepository;

        public FriendshipController(ApplicationDBContext context, IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all friendships in the database.
        /// </summary>
        /// <returns>A list of friendships</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var friendships = await _friendshipRepository.GetAllAsync();

            return Ok(friendships);
        }
        
        /// <summary>
        /// Retrieves a friendship in the database.
        /// </summary>
        /// <returns>A specific friendship</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(id);

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
            var createdFriendship = await _friendshipRepository.CreateAsync(newFriendship);
            return CreatedAtAction(nameof(GetById), new { id = newFriendship.Id }, FriendshipMapper.ToFriendshipDto(createdFriendship));

        }

        /// <summary>
        /// Updates a friendship in the database
        /// </summary>
        /// <returns>The updated friendship</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFriendshipDto friendshipDto)
        {

            if (!await _friendshipRepository.UserExists(friendshipDto.ReceiverId) || !await _friendshipRepository.UserExists(friendshipDto.RequesterId))
            {
                return BadRequest("Invalid requester or receiver ID.");
            }

            var userId = GetAuthenticatedUserId();
            if (userId != friendshipDto.ReceiverId)
            {
                return Unauthorized("Not authorized to update this friendship.");
            }

            var updatedFriendship = await _friendshipRepository.UpdateAsync(id, friendshipDto);
            if(updatedFriendship == null)
            {
                return NotFound("Friendship cannot be found");
            }

            return Ok(FriendshipMapper.ToFriendshipDto(updatedFriendship));
        }

        /// <summary>
        /// Deletes a friendship in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetAuthenticatedUserId();
            if (!await _friendshipRepository.UserCanDeleteFriendship(userId, id)) {
                return Unauthorized("You do not have permission to delete this friendship.");
            }
            
            await _friendshipRepository.DeleteAsync(id);
            return NoContent();
        }

        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }

    }
}