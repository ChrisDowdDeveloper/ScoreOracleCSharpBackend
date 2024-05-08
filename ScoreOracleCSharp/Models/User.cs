using Microsoft.AspNetCore.Identity;

namespace ScoreOracleCSharp.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ProfilePictureUrl {  get; set; } = string.Empty;

        // Relations
        public List<Friendship> ReceivedFriendships { get; set; } = new List<Friendship>();
        public List<Friendship> RequestedFriendships { get; set; } = new List<Friendship>();
        public List<GroupMember> GroupsJoined { get; set; } = new List<GroupMember>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<Prediction> PredictionsByPlayer { get; set; } = new List<Prediction>(); 
        public List<UserScore> UserScores { get; set; } = new List<UserScore>();


    }
}