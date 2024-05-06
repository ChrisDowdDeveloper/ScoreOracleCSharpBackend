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

        // Get All Notifications
        [HttpGet]
        public IActionResult GetAll() 
        {
            var notifications = _context.Notifications.ToList();
        
            return Ok(notifications);
        }

        // Get Notification By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var notification = _context.Notifications.Find(id);

            if(notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        // Create Notification
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

        // Update Notification
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            if (notificationDto.UserId.HasValue)
            {
                if (!await UserExists(notificationDto.UserId.Value))
                {
                    return BadRequest("User does not exist with that ID");
                }
                notification.UserId = notificationDto.UserId.Value;
            }

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

        private async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}