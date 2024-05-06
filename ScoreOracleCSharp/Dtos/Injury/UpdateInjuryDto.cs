using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Injury
{
    public class UpdateInjuryDto
    {
        [Required(ErrorMessage = "Player ID is required")]
        public int? PlayerId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Team ID is required")]
        public int? TeamId { get; set; }
        
    }
}