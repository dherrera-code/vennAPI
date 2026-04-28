using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class RoomDTO
    {
        // public int RoomId { get; set; }
        // RoomId is passed through the route!
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsRoomActive { get; set; }
        // public int UserId { get; set; }
        // public string? Hour { get; set; }


    }
}