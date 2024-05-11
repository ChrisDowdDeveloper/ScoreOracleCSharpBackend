using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(ApplicationDBContext context, INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all notifications in the database.
        /// </summary>
        /// <returns>A list of notifications</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] NotificationQueryObject query) 
        {
            var notifications = await _notificationRepository.GetAllAsync(query);
        
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a notification in the database.
        /// </summary>
        /// <returns>A specific notification</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if(notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        /// <summary>
        /// Creates a friendship in the database
        /// </summary>
        /// <returns>The created friendship</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto notificationDto)
        {
            if(!await _notificationRepository.UserExists(notificationDto.UserId))
            {
                return BadRequest("No user exists with that ID");
            }

            var newNotification = NotificationMapper.ToNotificationFromCreateDTO(notificationDto);
            var createdNotification = await _notificationRepository.CreateAsync(newNotification);
            return CreatedAtAction(nameof(GetById), new { id = newNotification.Id }, NotificationMapper.ToNotificationDto(createdNotification));
        }

        /// <summary>
        /// Updates a notification in the database
        /// </summary>
        /// <returns>The updated notification</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNotificationDto notificationDto)
        {
            if (!await _notificationRepository.UserExists(notificationDto.UserId))
            {
                return BadRequest("User does not exist with that ID");
            }

            var userId = GetAuthenticatedUserId();
            if(!await _notificationRepository.UserCanModifyNotification(userId, id))
            {
                return Unauthorized("You do not have permission to update this notification.");
            }

            var updatedNotification = await _notificationRepository.UpdateAsync(id, notificationDto);
            if(updatedNotification == null)
            {
                return NotFound("Notification cannot be found");
            }
            return Ok(NotificationMapper.ToNotificationDto(updatedNotification));
        }

        /// <summary>
        /// Deletes a notification in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetAuthenticatedUserId();
            if(!await _notificationRepository.UserCanModifyNotification(userId, id))
            {
                return Unauthorized("You do not have permission to delete this notification.");
            }
            await _notificationRepository.DeleteAsync(id);
            return NoContent();
        }

        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }

    }
}