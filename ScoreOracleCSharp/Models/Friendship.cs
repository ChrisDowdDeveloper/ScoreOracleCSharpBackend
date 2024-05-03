namespace ScoreOracleCSharp.Models
{
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blocked
    }
    public class Friendship
    {
        public int Id { get; set; }

        public int? RequesterId { get; set; }
        public User? Requester { get; set; }

        public int? ReceiverId { get; set; }
        public User? Receiver { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
        public DateTime? DateEstablished { get; set; }
    }
}