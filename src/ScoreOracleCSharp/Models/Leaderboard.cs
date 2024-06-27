using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    public enum LeaderboardType
    {
        PUBLIC,
        PRIVATE
    }
    [Table("Leaderboards")]
    public class Leaderboard
    {
        public int Id { get; set; }
        public LeaderboardType Type { get; set; } = LeaderboardType.PRIVATE;
        public string Name { get; set; } = string.Empty;

        public int? SportId { get; set; }
        public Sport? Sport { get; set; }

        //Relations
        public List<User> Users { get; set; } = new List<User>();
        public List<UserScore> ScoreByUser { get; set; } = new List<UserScore>();

    }
}
