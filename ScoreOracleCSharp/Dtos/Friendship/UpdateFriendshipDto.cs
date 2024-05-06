using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Friendship
{
    public class UpdateFriendshipDto
    {
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}