using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class PendingInvitationDTO
    {
        public int RoomId { get; set; }
        public string RoomTitle { get; set; }
        public string Category { get; set; }
        public int RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterIcon { get; set; }
    }
}