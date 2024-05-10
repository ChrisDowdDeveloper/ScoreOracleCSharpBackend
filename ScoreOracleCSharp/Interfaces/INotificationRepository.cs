using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllAsync(NotificationQueryObject query);
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification> CreateAsync(Notification notificationModel);
        Task<Notification?> UpdateAsync(int id, UpdateNotificationDto notificationDto);
        Task<Notification?> DeleteAsync(int id);
        Task<bool> UserExists(string userId);
        Task<bool> UserCanModifyNotification(string userId, int id);
    }
}