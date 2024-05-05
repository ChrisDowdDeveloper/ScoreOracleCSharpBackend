using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Notification
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public NotificationType Type { get; set; } = NotificationType.GAME_RESULT;
        public string Content { get; set; } = string.Empty;

    }
}