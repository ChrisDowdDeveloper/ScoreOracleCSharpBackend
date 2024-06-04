using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserRepository userRepository, ITokenService tokenService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null) 
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        // GET: api/user
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryObject query) 
        {
            var users = await _userRepository.GetAllUsersAsync(query);
            var userDtos = users.ConvertAll(user => UserMapper.ToUserDto(user));
            return Ok(userDtos);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = createUserDto.UserName,
                    Email = createUserDto.Email
                };

                var createdUser = await _userManager.CreateAsync(user, createUserDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = await _tokenService.CreateToken(user)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }


        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginUserDto.Email.ToLower());

            if (user == null) return Unauthorized("Invalid email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Email or password is incorrect");

            return Ok(new
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            });
        }

        // PATCH: api/user/{id}
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto userDto)
        {
            var userId = GetAuthenticatedUserId();
            if(userId != id)
            {
                return BadRequest("You cannot modify another person.");
            }
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userId = GetAuthenticatedUserId();
            if(userId != id)
            {
                return BadRequest("You cannot modify another person.");
            }
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

        private string GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (userId == null)
            {
                throw new InvalidOperationException("User must be authenticated.");
            }
            else
            {
                return userId;
            }
        }
    }
}
