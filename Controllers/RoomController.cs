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
    public class RoomController : ControllerBase
    {
        private readonly RoomServices _roomService;
        public RoomController(RoomServices roomServices)
        {
            _roomService = roomServices;   
        }

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

    }
}