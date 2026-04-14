using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class AvailabilityDTO
    {
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public AvailabilityStatus Status { get; set; }
    }
}