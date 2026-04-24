using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomMemberController : ControllerBase
    {
        // ENdpoints for Members of ROoms!!!
        private readonly RoomServices _roomService;
        public RoomMemberController(RoomServices roomServices)
        {
            _roomService = roomServices;
        }

        [HttpPost("InviteMemberToRoom")]
        public async Task<ActionResult<bool>> InviteMemberToRoom(RoomMemberDTO newRoomMember)
        {
            // this endpoint will add an instance to the database
            var result = await _roomService.InviteMemberToRoom(newRoomMember);
            if(!result) throw new InvalidDataException("Invite is pending or member has joined the room!");

            return true;
        }

        [HttpGet("GetAllMembersByRoomId/{roomId}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllMembersByRoomId(int roomId)
        {
            // this needs to be queried when calling getRoomsById .where isDeleted is false!
            return await _roomService.GetAllJoinedMembersByRoom(roomId);
        }

        [HttpGet("GetUserInvitationByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<RoomMember>>> GetUsersInvitesByUser(int userId)
        {
            // this function will return details of room name, 
            var invitesList = await _roomService.GetPendingInvitesByUserId(userId);

            if(invitesList == null) return NotFound();

            return invitesList;
        }

        [HttpPut("ChangeMemberStatusToAccepted")]
        public async Task<ActionResult<bool>> UpdateMemberStatusToAccepted(RoomMemberDTO roomMember)
        {
            var result = await _roomService.ChangeMemberStatusToAccepted(roomMember);
            if (result == null) return BadRequest();

            return result;
        }

        [HttpPut("RemoveMemberFromRoom")]
        public async Task<ActionResult<bool>> RemoveMemberFromRoom(RoomMemberDTO memberToRemove)
        {
            var result = await _roomService.RemoveMemberFromRoom(memberToRemove);
            if(!result) return BadRequest();

            return result;
        }
    }
}