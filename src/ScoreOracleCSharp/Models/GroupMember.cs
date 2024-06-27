using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreOracleCSharp.Models
{
    [Table("GroupMembers")]
    public class GroupMember
    {
        public int Id { get; set; }
        
        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public DateOnly JoinedAt { get; set; }
    }
}
