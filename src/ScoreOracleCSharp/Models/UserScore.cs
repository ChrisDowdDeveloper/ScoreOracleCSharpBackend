using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    [Table("UserScores")]
    public class UserScore
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public int? LeaderboardId { get; set; }
        public Leaderboard? Leaderboard { get; set; }

        public int Score { get; set; }
        public DateTime UpdatedLast { get; set; } = DateTime.Now;
    }
}
