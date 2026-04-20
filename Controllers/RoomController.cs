
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
            if(string.IsNullOrWhiteSpace(room.Title) || string.IsNullOrWhiteSpace(room.Category))
            {
                return BadRequest("Title and Category is required!");
            }
            var success = await _roomService.AddNewRoomAsync(room);

            if (success) return Ok(new {success});

            return BadRequest($"Room Data is invalid or user id: {room.UserId} may not exist!");
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
        [HttpGet("GetCreatedAndJoinedRoomsByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<RoomModel>>> GetRelevantRoomsByUserIdAsync(int id)
        {
            var roomsList = await _roomService.GetRelevantRoomsByUserIdAsync(id);
            return roomsList;
        }

        [HttpPut("UpdateRoom/{id}")]
        public async Task<ActionResult<RoomModel>> UpdateRoom(int id,[FromBody] RoomModel updatedRoom)
        {
            return await _roomService.UpdateRoomAsync(id, updatedRoom);
        }
        [HttpPut("DeleteRoomById/{id}")]
        public async Task<ActionResult<bool>> RemoveRoom(int id)
        {
            var result =  await _roomService.RemoveRoomByIdAsync(id);
            if(!result) return NotFound($"Room By id: {id} doesn't exist");

            return Ok(true);

        }

    }
}