using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Dtos.Notification;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Dtos.UserScore;

namespace ScoreOracleCSharp.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public int FriendshipCount { get; set; }
        public int GroupMembershipCount { get; set; }
        public int NotificationCount { get; set; }
        public List<FriendshipDto> ReceivedFriendships { get; set; }
        public List<FriendshipDto> RequestedFriendships { get; set; }
        public List<GroupMemberDto> GroupsJoined { get; set; }
        public List<NotificationDto> Notifications { get; set; }
        public List<PredictionDto> PredictionsByPlayer { get; set; }
        public List<UserScoreDto> UserScores { get; set; }
    }
}