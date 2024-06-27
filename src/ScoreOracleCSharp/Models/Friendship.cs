using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blocked
    }
    [Table("Friendships")]
    public class Friendship
    {
        public int Id { get; set; }

        public string? RequesterId { get; set; }
        public User? Requester { get; set; }

        public string? ReceiverId { get; set; }
        public User? Receiver { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
        public DateTime? DateEstablished { get; set; }
    }
}