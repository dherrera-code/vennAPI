using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vennAPI.Context;
using vennAPI.Models;

namespace vennAPI.Services
{
    public class RoomServices
    {
        private readonly DataContext _dataContext;
        public RoomServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddNewRoomAsync(RoomModel room)
        {
            await _dataContext.Rooms.AddAsync(room);
            return await _dataContext.SaveChangesAsync() != 0;
        }
        public async Task<List<RoomModel>> GetAllRoomsAsync() => await _dataContext.Rooms.ToListAsync();
        
        
    }
}