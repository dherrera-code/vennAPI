using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomId { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public DateTime? EventDate { get; set; }
        // public TimeOnly? GoldenHour { get; set; }
        public bool IsRoomActive { get; set; }
        public int UserId { get; set; } // Foreign key of the room creator!

    }
}