using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.User
{
    public class CreateUserRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; } = new DateOnly();
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ProfilePictureUrl {  get; set; } = string.Empty;

    }
}