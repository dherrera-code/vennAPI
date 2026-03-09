using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public DateTime EventDate { get; set; }
        // public string? RoomPath { get; set; }
        public List<int>? UsersInRoomList { get; set; }
        public TimeOnly GoldenHour { get; set; }
        public bool IsRoomActive { get; set; }

    }
}