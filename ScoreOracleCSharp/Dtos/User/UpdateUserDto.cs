using System;
using System.ComponentModel.DataAnnotations;

namespace ScoreOracleCSharp.Dtos.User
{
    public class UpdateUserDto
    {
        [Required, MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
