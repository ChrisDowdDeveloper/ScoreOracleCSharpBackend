using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Mappers;

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
        public IActionResult Create([FromBody] CreateNotificationDto notificationDto)
        {
            if(!UserExists(notificationDto.UserId))
            {
                return BadRequest("No user exists with that ID");
            }

            var newNotification = NotificationMapper.ToNotificationFromCreateDTO(notificationDto);
            _context.Notifications.Add(newNotification);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newNotification.Id }, NotificationMapper.ToNotificationDto(newNotification));
        }

        private bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}