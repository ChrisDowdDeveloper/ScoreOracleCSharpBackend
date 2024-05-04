using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using ScoreOracleCSharp.Dtos.User;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Username = userModel.Username,
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                ProfilePictureUrl = userModel.ProfilePictureUrl,
                DateCreated = userModel.DateCreated,
                FriendshipCount = userModel.ReceivedFriendships.Count + userModel.RequestedFriendships.Count,
                GroupMembershipCount = userModel.GroupsJoined.Count,
                NotificationCount = userModel.Notifications.Count(n => !n.IsRead) 
            };
        }

        public static User ToUserFromCreateDTO(CreateUserRequestDto userDto)
        {
            return new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                DateOfBirth = userDto.DateOfBirth,
                DateCreated = userDto.DateCreated,
                ProfilePictureUrl = userDto.ProfilePictureUrl
            };
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

}