using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users in the database.
        /// </summary>
        /// <returns>A list of userse</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var users = await _context.Users.ToListAsync();
        
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user in the database.
        /// </summary>
        /// <returns>A specific user</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a user in the database
        /// </summary>
        /// <returns>The created user</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestDto createUserDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
            {
                return BadRequest("Email already in use.");
            }
            if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
            {
                return BadRequest("Username already in use.");
            }

            var newUser = UserMapper.ToUserFromCreateDTO(createUserDto);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, UserMapper.ToUserDto(newUser));
        }

        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <returns>The updated user</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            /*if (id != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to update this user.");
            }
            */

            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            if (await _context.Users.AnyAsync(u => (u.Email == userDto.Email || u.Username == userDto.Username) && u.Id != id))
            {
                return BadRequest("Username or email already exists.");
            }

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.DateOfBirth = userDto.DateOfBirth;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl;

            await _context.SaveChangesAsync();

            return Ok(UserMapper.ToUserDto(user));
        }

        /// <summary>
        /// Deletes a user in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to delete this user.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found and could not be deleted.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }
    }
}