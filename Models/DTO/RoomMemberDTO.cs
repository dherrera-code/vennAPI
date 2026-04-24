using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class RoomMemberDTO
    {
        public int RoomModelId { get; set; } // Foreign Key
        public int MemberId { get; set; } // Foreign Key
        // public Boolean IsAdmin { get; set; } 
        public bool? IsAccepted { get; set; } // If False, invite is sent BUT member hasn't accepted invitation else; user accepted room invitation!
    }
}