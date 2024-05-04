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
        public int RequesterId { get; set; }
        public string RequesterUsername { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverUsername { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DateEstablished { get; set; }
    }
}