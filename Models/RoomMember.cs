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
        public int UserModelId { get; set; } // Foreign Key
        public bool IsAccepted { get; set; }
        public bool IsDeleted { get; set; } = false;

        public RoomModel Room {get; set; }
        public UserModel MemberInfo {get; set;}
    }
}