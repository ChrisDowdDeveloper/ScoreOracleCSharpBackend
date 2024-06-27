using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    [Table("Players")]
    public class Player
    {
        public int Id { get; set; }
        
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public List<Injury> PlayerInjury { get; set; } = new List<Injury>();
    }
}
