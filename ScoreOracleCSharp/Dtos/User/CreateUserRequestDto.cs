using System;
using System.ComponentModel.DataAnnotations;

namespace ScoreOracleCSharp.Dtos.User
{
    public class CreateUserRequestDto
    {
        [Required, MinLength(3)]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
