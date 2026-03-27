using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomMemberModel
    {
        // columns needed for room members: FK: roomId, Fk: userId, 
        [ForeignKey("RoomModelId")]
        public int RoomModelId { get; set; }
        [ForeignKey("UserModelId")]
        public int UserModelId { get; set; }
        
    }
}