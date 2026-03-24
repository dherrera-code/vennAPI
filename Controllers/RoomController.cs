using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize]
    public class RoomController(RoomServices roomServices) : ControllerBase
    {
        private readonly RoomServices _roomService = roomServices;

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom(RoomModel room)
        {
            if(room == null)
            {
                return BadRequest("Room data is required.");
            }
            var success = await _roomService.AddNewRoomAsync(room);

            if (success) return Ok(new {success});

            return BadRequest(new {success});
        }

        [HttpGet("GetRoomByRoomId/{roomId}")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            var room = await _roomService.GetRoomByRoomIdAsync(roomId);

            if (room != null) return Ok(new {room}); 

            return NotFound(new { message = "No room found!"});
        }

        [HttpGet("GetAllRooms")]
        public async Task<ActionResult<IEnumerable<RoomModel>>> GetAllRooms()
        {
            return await _roomService.GetAllRooms();
        }

        [HttpGet("GetCreatedRoomByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<RoomModel>>> GetCreatedRoomsById(int userId)
        {
            return await _roomService.GetRoomsByUserIdAsync(userId);
        }

        [HttpPut("UpdateRoom/{id}")]
        public async Task<ActionResult<RoomModel>> UpdateRoom(int id,[FromBody] RoomModel updatedRoom)
        {
            return await _roomService.UpdateRoomAsync(id, updatedRoom);
        }

    }
}