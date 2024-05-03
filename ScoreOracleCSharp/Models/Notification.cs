namespace ScoreOracleCSharp.Models
{
    public enum NotificationType
    {
        GAME_RESULT,
        FRIEND_REQUEST,
        FRIEND_RESPONSE,
        PREDICTION_REMINDER
    }
    public class Notification
    {
        public int Id { get; set; }
            
        public int? UserId { get; set; }
        public User? User { get; set; }

        public NotificationType Type { get; set; } = NotificationType.GAME_RESULT;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
