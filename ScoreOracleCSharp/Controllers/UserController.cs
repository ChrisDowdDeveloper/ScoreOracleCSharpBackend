using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryObject query) 
        {
            var users = await _userRepository.GetAllUsersAsync(query);
            var userDtos = users.ConvertAll(user => UserMapper.ToUserDto(user));
            return Ok(userDtos);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
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
            try
            {
                var createdUser = await _userRepository.CreateUserAsync(user, createUserDto.Password);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, UserMapper.ToUserDto(createdUser));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginUserDto.Email);
            if(user == null)
            {
                return BadRequest("Invalid email or password");
            }
            var passwordValid = await _userRepository.CheckPasswordAsync(user, loginUserDto.Password);
            if(passwordValid)
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
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userDto.Username ?? user.UserName;
            user.Email = userDto.Email ?? user.Email;
            user.FirstName = userDto.FirstName ?? user.FirstName;
            user.LastName = userDto.LastName ?? user.LastName;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl ?? user.ProfilePictureUrl;

            try
            {
                var updatedUser = await _userRepository.UpdateUserAsync(user);
                return Ok(UserMapper.ToUserDto(updatedUser));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            bool deleted = await _userRepository.DeleteUserAsync(id);
            if (deleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound("User not found.");
            }
        }

        // Helper method to get authenticated user ID
        private string? GetAuthenticatedUserId()
        {
            return User.Identity?.Name ?? throw new InvalidOperationException("User must be authenticated.");
        }
    }
}
