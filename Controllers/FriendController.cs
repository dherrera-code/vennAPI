using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
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
            return _friendService.SendFriendRequest(requesterId, receiverId);
        }
    }
}