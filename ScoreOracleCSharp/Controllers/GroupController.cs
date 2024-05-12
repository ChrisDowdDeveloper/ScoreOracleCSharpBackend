using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<GroupController> _logger;

        public GroupController(ILogger<GroupController> logger, ApplicationDBContext context, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all groups in the database.
        /// </summary>
        /// <returns>A list of groups</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GroupQueryObject query)
        {
            var groups = await _groupRepository.GetAllAsync(query);
            return Ok(groups);
        }

        /// <summary>
        /// Retrieves a specific group from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the group to retrieve.</param>
        /// <returns>A specific group</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        /// <summary>
        /// Creates a group in the database.
        /// </summary>
        /// <param name="groupDto">Data transfer object for creating a group.</param>
        /// <returns>The created group</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto groupDto)
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("User ID from token: {UserId}", userId);

            if (userId == null)
            {
                return Unauthorized("You are not authorized to create a group.");
            }

            if (!await _groupRepository.UserExists(groupDto.CreatedByUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            var newGroup = GroupMapper.ToGroupFromCreateDTO(groupDto);
            var createdGroup = await _groupRepository.CreateAsync(newGroup);
            return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, GroupMapper.ToGroupDto(createdGroup));
        }

        /// <summary>
        /// Updates a group in the database.
        /// </summary>
        /// <param name="id">The ID of the group to update.</param>
        /// <param name="groupDto">Data transfer object for updating a group.</param>
        /// <returns>The updated group</returns>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGroupDto groupDto)
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Attempting to update group with User ID from token: {UserId}", userId);

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            if (!await _groupRepository.UserCanModifyGroup(userId, id))
            {
                return Unauthorized("You do not have permission to update this group.");
            }

            if (string.IsNullOrWhiteSpace(groupDto.Name) || groupDto.Name.Length < 3)
            {
                return BadRequest("Group name must have at least 3 characters and cannot be empty.");
            }

            var updatedGroup = await _groupRepository.UpdateAsync(id, groupDto);
            if (updatedGroup == null)
            {
                return NotFound("Group cannot be found.");
            }

            return Ok(GroupMapper.ToGroupDto(updatedGroup));
        }

        /// <summary>
        /// Deletes a group in the database.
        /// </summary>
        /// <param name="id">The ID of the group to delete.</param>
        /// <returns>HTTP 204 No Content status code if successful</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Attempting to delete group with User ID from token: {UserId}", userId);

            if (!await _groupRepository.UserCanModifyGroup(userId, id))
            {
                return Unauthorized("You do not have permission to delete this group");
            }

            await _groupRepository.DeleteAsync(id);
            return NoContent();
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
