using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Notification
{
    public class UpdateNotificationDto
    {
        public int? UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool? IsRead { get; set; }
    }
}