using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class UserAvailability
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key
        public UserModel User { get; set; }
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }  // 0 - 23
        public AvailabilityStatus Status { get; set; }

    }

    public enum AvailabilityStatus
    {
        No = 0,
        Maybe = 1,
        Yes
    }
}