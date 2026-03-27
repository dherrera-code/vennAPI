using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class CreateFriendDTO
    {
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public FriendshipStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}