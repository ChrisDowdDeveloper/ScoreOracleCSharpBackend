using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() 
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = users.Select(user => UserMapper.ToUserDto(user)).ToList();
            return Ok(userDtos);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(UserMapper.ToUserDto(user));
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequestDto createUserDto)
        {
            var user = UserMapper.ToUserFromCreateDTO(createUserDto);
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, UserMapper.ToUserDto(user));
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if(user == null)
            {
                return BadRequest("Invalid email or password");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password, false, false);
            if(result.Succeeded)
            {
                var userDto = UserMapper.ToUserDto(user);
                return Ok(userDto);
            }
            else
            {
                return BadRequest("Invalid email or password");
            }
        }

        // PATCH: api/user/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update the user properties
            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(UserMapper.ToUserDto(user));
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // Helper method to get authenticated user ID
        private string? GetAuthenticatedUserId()
        {
            return User.Identity?.Name ?? throw new InvalidOperationException("User must be authenticated.");
        }
    }
}
