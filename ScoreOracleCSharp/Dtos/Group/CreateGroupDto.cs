using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Group
{
    public class CreateGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}