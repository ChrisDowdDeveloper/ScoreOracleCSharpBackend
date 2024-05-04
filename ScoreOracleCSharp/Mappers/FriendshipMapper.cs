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
                RequesterId = friendshipModel.RequesterId ?? 0,
                RequesterUsername = friendshipModel.Requester?.Username ?? "Unknown",
                ReceiverId = friendshipModel.ReceiverId ?? 0,
                ReceiverUsername = friendshipModel.Receiver?.Username ?? "Unknown",
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