namespace ScoreOracleCSharp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? CreatedByUserId { get; set; }
        public User? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

    }
}

