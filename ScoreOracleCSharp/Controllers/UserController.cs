using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        // Get All Users

        [HttpGet]
        public IActionResult GetAll() 
        {
            var users = _context.Users.ToList().Select(u => UserMapper.ToUserDto(u));
        
            return Ok(users);
        }

        // Get User By ID

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var user = _context.Users.Find(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Create User
        [HttpPost]
        public IActionResult Register([FromBody] CreateUserRequestDto createUserDto)
        {
            if (_context.Users.Any(u => u.Email == createUserDto.Email))
            {
                return BadRequest("Email already in use.");
            }
            if (_context.Users.Any(u => u.Username == createUserDto.Username))
            {
                return BadRequest("Username already in use.");
            }

            var newUser = UserMapper.ToUserFromCreateDTO(createUserDto);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, UserMapper.ToUserDto(newUser));
        }

        // Update User
        [HttpPut]
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

            if (_context.Users.Any(u => (u.Email == userDto.Email || u.Username == userDto.Username) && u.Id != id))
            {
                return BadRequest("Username or email already exists.");
            }

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.DateOfBirth = userDto.DateOfBirth;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl;

            _context.SaveChanges();

            return Ok(new { message = "User updated successfully." });
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }
    }
}