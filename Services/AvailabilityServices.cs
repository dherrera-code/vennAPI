using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vennAPI.Context;
using vennAPI.Models;
using vennAPI.Models.DTO;

namespace vennAPI.Services
{
    public class AvailabilityServices(DataContext context)
    {
        private readonly DataContext _Context = context;

        public async Task<ActionResult<IEnumerable<UserAvailability>>> AddNewAvailability(int userId, AvailabilityDTO[] availability)
        {
            // First check if rows for user already exist.
            //If rows don't exist, then create new rows 
            // ELSE Remove rows with .RemoveRange then await save changes! THEN Create new rows!
            // return new rows
            var existingAvailability = await GetAllAvailabilityByUserId(userId);
            if(existingAvailability.Count > 0)
            {
                // meaning if rows exist!
                // Remove List
                _Context.UserAvailability.RemoveRange(existingAvailability);
                await _Context.SaveChangesAsync();
            }
            // Create new rows!
            var newWeekAvail = availability.Select( entry => new UserAvailability
            {
                UserId = userId,
                Day = entry.Day,
                Hour = entry.Hour,
                Status = entry.Status
            }).ToList();

             _Context.UserAvailability.AddRange(newWeekAvail);
            await _Context.SaveChangesAsync();

            return newWeekAvail;
        }

        private async Task<List<UserAvailability>> GetAllAvailabilityByUserId(int userId)
        {
            return await _Context.UserAvailability.AsNoTracking().Where(user => user.UserId == userId).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<UserAvailability>>> GetWeeklyAvailabilityByUserIdAsync(int userId)
        {
            return await _Context.UserAvailability.AsNoTracking().Where(user => user.UserId == userId).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<UserAvailability>>> GetDailyAvailabilityById(DayOfWeek dayOfWeek, int userId)
        {
            return await _Context.UserAvailability.AsNoTracking().Where(user => user.UserId == userId && user.Day == dayOfWeek).ToListAsync();
        }
    }
}