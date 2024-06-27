using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    [Table("Injuries")]
    public class Injury
    {
        public int Id { get; set; }

        public int? PlayerId { get; set; }
        public Player? Player { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
