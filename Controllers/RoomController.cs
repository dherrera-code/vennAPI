using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
using vennAPI.Models.DTO;
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

        // ENdpoints for Members of ROoms!!!

        [HttpPost("InviteMemberToRoom")]
        public async Task<ActionResult<RoomMemberDTO>> InviteMemberToRoom(RoomMemberDTO newRoomMember)
        {
            // this endpoint will add an instance to the database
            var result = await _roomService.InviteMemberToRoom(newRoomMember);
            if(result == null) throw new InvalidDataException("Invite is pending or member has joined the room!");

            return result;
        }

        [HttpGet("GetAllMembersByRoomId/{roomId}")]
        public async Task<ActionResult<IEnumerable<RoomMember>>> GetAllMembersByRoomId(int roomId)
        {
            // this needs to be queried when calling getRoomsById .where isDeleted is false!
            return await _roomService.GetAllJoinedMembersByRoom(roomId);
        }

        [HttpGet("GetUserInvitationByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<RoomInviteDTO>>> GetUsersInvitesByUser(int userId)
        {
            // this function will return details of room name, 
            throw new NotImplementedException();
        }

        [HttpPut("ChangeMemberStatusToAccepted")]
        public async Task<ActionResult<bool>> UpdateMemberStatusToAccepted(RoomMemberDTO roomMember)
        {
            var result = await _roomService.ChangeMemberStatusToAccepted(roomMember);
            if (result == null) return BadRequest();

            return result;
        }

        [HttpPut("RemoveMemberFromRoom/{memberId}")]
        public async Task<ActionResult<bool>> RemoveMemberFromRoom(RoomMemberDTO memberToRemove)
        {
            var result = await _roomService.RemoveMemberFromRoom(memberToRemove);
            if(!result) return BadRequest();

            return result;
        }

    }
}