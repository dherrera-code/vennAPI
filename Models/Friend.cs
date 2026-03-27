using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public FriendshipStatus MyProperty { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public DateTime? AcceptedAt { get; set; }
    }

    public enum FriendshipStatus
    {
        Pending, // 0
        Accepted, // 1
        Deleted
    }
}