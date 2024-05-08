using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public NotificationController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all notifications in the database.
        /// </summary>
        /// <returns>A list of notifications</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var notifications = await _context.Notifications.ToListAsync();
        
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a notification in the database.
        /// </summary>
        /// <returns>A specific notification</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

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
            if(!await UserExists(notificationDto.UserId))
            {
                return BadRequest("No user exists with that ID");
            }

            var newNotification = NotificationMapper.ToNotificationFromCreateDTO(notificationDto);
            _context.Notifications.Add(newNotification);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newNotification.Id }, NotificationMapper.ToNotificationDto(newNotification));
        }

        /// <summary>
        /// Updates a notification in the database
        /// </summary>
        /// <returns>The updated notification</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            if (!await UserExists(notificationDto.UserId))
            {
                return BadRequest("User does not exist with that ID");
            }
            notification.UserId = notificationDto.UserId;

            if (!string.IsNullOrWhiteSpace(notificationDto.Type))
            {
                if (Enum.TryParse<NotificationType>(notificationDto.Type, true, out var parsedType))
                {
                    notification.Type = parsedType;
                }
                else
                {
                    return BadRequest("Invalid notification type specified.");
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDto.Content))
            {
                if (notificationDto.Content.Length <= 1)
                {
                    return BadRequest("Notification must contain content.");
                }
                notification.Content = notificationDto.Content;
            }

            if (notificationDto.IsRead.HasValue)
            {
                notification.IsRead = notificationDto.IsRead.Value;
            }

            await _context.SaveChangesAsync();
            return Ok(NotificationMapper.ToNotificationDto(notification));
        }

        /// <summary>
        /// Deletes a notification in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if(notification == null)
            {
                return NotFound("Notification not found and could not be deleted");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> UserExists(string userId) 
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}