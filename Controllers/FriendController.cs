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
    public class FriendController(FriendService friendService) : ControllerBase
    {
        private readonly FriendService _friendService = friendService;

        [HttpPost("SendFriendRequest/{requesterId}/{receiverId}")]
        public async Task<ActionResult<Friend>> SendFriendRequest(int requesterId, int receiverId)
        {
            var entry = await _friendService.SendFriendRequest(requesterId, receiverId);
            if(entry == null) return BadRequest(new{message = "Friend Request already created!"});
            return entry;
        }
        [HttpGet("GetAllPendingFriends/{userId}")]
        public async Task<ActionResult<List<Friend>>> GetAllPendingFriend(int userId)
        {
            return await _friendService.GetPendingFriendAsync(userId);
        }
        [HttpGet("GetAllAcceptedFriends/{userId}")]
        public async Task<ActionResult<Friend>> GetAllAcceptedFriend(int userId)
        {
            return _friendService.GetAcceptedFriendAsync(userId);
        }
        [HttpPut("AddFriendStatusByColumnId/{id}")]
        public async Task<ActionResult<Friend>> AddFriendStatus(int id)
        {
            return _friendService.AddFriendStatus(id);
        }
    }
}