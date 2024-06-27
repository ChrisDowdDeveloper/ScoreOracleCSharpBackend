using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Friendship
{
    public class FriendshipDto
    {
        public int Id { get; set; }
        public string RequesterId { get; set; } = string.Empty;
        public string RequesterUsername { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string ReceiverUsername { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DateEstablished { get; set; }
    }
}