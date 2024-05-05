using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDto ToNotificationDto(Notification notificationModel)
        {
            return new NotificationDto
            {
                Id = notificationModel.Id,
                Type = notificationModel.Type.ToString(),
                Content = notificationModel.Content,
                CreatedAt = notificationModel.CreatedAt,
                IsRead = notificationModel.IsRead
            };
        }

        public static Notification ToNotificationFromCreateDTO(CreateNotificationDto notificationDto)
        {
            return new Notification
            {
                UserId = notificationDto.UserId,
                Type = notificationDto.Type,
                Content = notificationDto.Content,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
        }
    }
}