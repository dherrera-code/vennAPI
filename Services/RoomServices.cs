using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<RoomModel>> GetAllRoomsAsync() => await _dataContext.Rooms.ToListAsync();
        public async Task<RoomModel> GetRoomByRoomIdAsync(int roomId)
        {
            return await _dataContext.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetAllRooms()
        {
            return await _dataContext.Rooms.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetRoomsByUserIdAsync(int userId)
        {
            var rooms = await _dataContext.Rooms.Where(room => room.UserId == userId).AsNoTracking().ToListAsync();
            return rooms;
        }

        public async Task<ActionResult<RoomModel>> UpdateRoomAsync(int id, RoomModel updatedRoom)
        {
            var findRoom = await GetRoomByRoomIdAsync(id);

            if(findRoom == null) return null;

            findRoom.Title = updatedRoom.Title;
            findRoom.Category = updatedRoom.Category;
            findRoom.EventDate = updatedRoom.EventDate;
            findRoom.IsRoomActive = updatedRoom.IsRoomActive;

            _dataContext.Rooms.Update(findRoom);
            await _dataContext.SaveChangesAsync();
            return findRoom;
        }
    }
}