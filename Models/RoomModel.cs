using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomId { get; set; } // Primary Key!!
        public string? Title { get; set; }
        public string? Category { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly? GoldenHour { get; set; }
        //public string? Hour { get; set; } //adding this later
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IsRoomActive { get; set; } 

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("UserId")]
        public UserModel? UserModel {get; set;}
        public int UserId { get; set; } // Foreign key of the room creator!
        // Navigation property for our RoomMembers

        public ICollection<RoomMember> Members {get; set;} = [];
    }
}