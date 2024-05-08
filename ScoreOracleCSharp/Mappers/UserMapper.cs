using System;
using System.Linq;
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
                UserName = userModel.UserName,
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
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                DateCreated = DateTime.UtcNow,
                ProfilePictureUrl = userDto.ProfilePictureUrl
            };
        }
    }
}
