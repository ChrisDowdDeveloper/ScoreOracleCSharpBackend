using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}