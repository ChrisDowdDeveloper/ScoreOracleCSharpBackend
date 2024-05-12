using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDBContext _context;
        public NotificationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateAsync(Notification notificationModel)
        {
            await _context.Notifications.AddAsync(notificationModel);
            await _context.SaveChangesAsync();
            return notificationModel;
        }

        public async Task<Notification?> DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if(notification == null)
            {
                return null;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<List<Notification>> GetAllAsync(NotificationQueryObject query)
        {
            var notifications = _context.Notifications.Include(n => n.User).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.UserName))
            {
                notifications = notifications.Where(n => n.User != null && n.User.UserName != null && n.User.UserName.Contains(query.UserName));
            }

            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                var typeStr = query.Type.ToUpper();
                if (Enum.TryParse<NotificationType>(query.Type, out var typeEnum))
                {
                    notifications = notifications.Where(n => n.Type == typeEnum);
                }
                else
                {
                    throw new ArgumentException("Invalid notification type specified");
                }
            }

            if(query.IsRead)
            {
                notifications = notifications.Where(n => n.IsRead == query.IsRead);
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("UserName", StringComparison.OrdinalIgnoreCase))
                {
                    notifications = query.IsDescending 
                            ? notifications.OrderByDescending(n => 
                                n.User != null ? n.User.UserName : "") 
                            : notifications.OrderBy(n => 
                                n.User != null ? n.User.UserName : "");
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await notifications
                        .Skip(skipNumber)
                        .Take(query.PageSize)
                        .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<Notification?> UpdateAsync(int id, UpdateNotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if(notification == null)
            {
                return null;
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
                    throw new ArgumentException("Invalid notification type specified.");
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDto.Content))
            {
                if (notificationDto.Content.Length <= 1)
                {
                    throw new Exception("Notification must contain content.");
                }
                notification.Content = notificationDto.Content;
            }

            if (notificationDto.IsRead.HasValue)
            {
                notification.IsRead = notificationDto.IsRead.Value;
            }
            
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> UserCanModifyNotification(string userId, int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if(notification == null)
            {
                return false;
            }
            return notification.UserId == userId;
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}