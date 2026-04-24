using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomMember
    {
        public int Id { get; set; }
        public int RoomModelId { get; set; } // Foreign Key
        public int? UserModelId { get; set; } // Foreign Key
        public bool IsAccepted { get; set; } // If False, invite is sent BUT member hasn't accepted invitation else; user accepted room invitation!
        public bool IsDeleted { get; set; } = false;

        public RoomModel Room {get; set; }// Gets room details
        public UserModel MemberInfo {get; set;} //Get info of user, Add data for availability
    }
}