using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Helpers
{
    public class NotificationQueryObject
    {
        public string? UserName { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; }
    }
}