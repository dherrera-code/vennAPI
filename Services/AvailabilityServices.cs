using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Context;
using vennAPI.Models.DTO;

namespace vennAPI.Services
{
    public class AvailabilityServices(DataContext context)
    {
        private readonly DataContext _Context = context;

        public async Task<ActionResult> AddNewAvailability(AvailabilityDTO[] availability)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<IEnumerable<AvailabilityDTO>>> GetWeeklyAvailabilityByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<IEnumerable<AvailabilityDTO>>> GetDailyAvailabilityById(DayOfWeek dayOfWeek, int userId)
        {
            throw new NotImplementedException();
        }
    }
}