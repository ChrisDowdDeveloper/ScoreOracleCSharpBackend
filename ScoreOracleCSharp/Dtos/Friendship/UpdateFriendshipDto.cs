using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Friendship
{
    public class UpdateFriendshipDto
    {
        public string RequesterId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public FriendshipStatus Status { get; set; }
    }
}