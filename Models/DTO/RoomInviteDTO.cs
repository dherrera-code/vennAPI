using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class RoomInviteDTO
    {
        public int RoomId { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        
    }
}