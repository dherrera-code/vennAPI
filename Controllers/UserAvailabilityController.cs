using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAvailabilityController(AvailabilityServices services) : ControllerBase
    {
        private readonly AvailabilityServices _availability = services;
        // This function will return availability after creation
        [HttpPost("CreateWeeklyAvailabilityByUserId")]
        public async Task<ActionResult> CreateUserAvailability(AvailabilityDTO[] availability)
        {
            var result = await _availability.AddNewAvailability(availability);

            if(result == null) throw new InvalidDataException("Data was invalid!");

            return result; 

        }

        [HttpGet("GetUserWeeklyAvailabilityByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<AvailabilityDTO>>> GetWeeklyAvailability(int userId)
        {
            var result = await _availability.GetWeeklyAvailabilityByUserIdAsync(userId);
            return result;

        }
        
        [HttpGet("GetUserAvailabilityByDayOfWeek/{dayOfWeek}/{userId}")]
        public async Task<ActionResult<IEnumerable<AvailabilityDTO>>>
        GetAvailabilityByDayAndUser(DayOfWeek dayOfWeek, int userId)
        {
            var result = await _availability.GetDailyAvailabilityById(dayOfWeek, userId);
            return result;
        }
    }
}