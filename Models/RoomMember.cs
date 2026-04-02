using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomMember
    {
        //Not in Database Yet
        public int Id { get; set; }
        public int RoomModelId { get; set; } // Foreign Key

        [ForeignKey("UserModelId")]
        public int UserModelId { get; set; } // Foreign Key
        // public Boolean IsAdmin { get; set; } 
        public bool IsAccepted { get; set; } // If False, invite is sent BUT member hasn't accepted invitation else; user accepted room invitation!
        public bool IsDeleted { get; set; } = false;
    }
}