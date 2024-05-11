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
                NotificationCount = userModel.Notifications.Count(n => !n.IsRead),
                ReceivedFriendships = userModel.ReceivedFriendships.Select(rec => FriendshipMapper.ToFriendshipDto(rec)).ToList(),
                RequestedFriendships = userModel.RequestedFriendships.Select(req => FriendshipMapper.ToFriendshipDto(req)).ToList(),
                GroupsJoined = userModel.GroupsJoined.Select(m => GroupMemberMapper.ToGroupMemberDto(m)).ToList(),
                Notifications = userModel.Notifications.Select(n => NotificationMapper.ToNotificationDto(n)).ToList(),
                PredictionsByPlayer = userModel.PredictionsByPlayer.Select(p => PredictionMapper.ToPredictionDto(p)).ToList(),
                UserScores = userModel.UserScores.Select(us => UserScoreMapper.ToUserScoreDto(us)).ToList()
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
