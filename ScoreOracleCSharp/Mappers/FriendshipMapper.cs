using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class FriendshipMapper
    {
        public static FriendshipDto ToFriendshipDto(this Friendship friendshipModel)
        {
            return new FriendshipDto 
            {
                Id = friendshipModel.Id,
                RequesterId = friendshipModel.RequesterId ?? "Unknown",
                RequesterUsername = friendshipModel.Requester?.UserName ?? "Unknown",
                ReceiverId = friendshipModel.ReceiverId ?? "Unknown",
                ReceiverUsername = friendshipModel.Receiver?.UserName ?? "Unknown",
                Status = friendshipModel.Status.ToString(),
                DateEstablished = friendshipModel.DateEstablished
            };
        }

        public static Friendship ToFriendshipFromCreateDTO(CreateFriendshipDto friendshipDto)
        {
            return new Friendship
            {
                RequesterId = friendshipDto.RequesterId,
                ReceiverId = friendshipDto.ReceiverId,
                Status = FriendshipStatus.Pending,
                DateEstablished = null
            };
        }
    }
}