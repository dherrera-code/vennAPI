using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProfileController(UserServices userServices) : ControllerBase
    {
        
        private readonly UserServices _userService = userServices;

        // In the future, I may move both profile endpoints to another controller! For Authentication purposes!

        [HttpGet("GetProfileByUserId/{id}")]
        public async Task<ActionResult<ProfileDTO>> GetUserProfileByUserId(int id)
        {
            return await _userService.GetUserProfileByIdAsync(id);
        }

        [HttpPut("UpdateUserProfileByUserId/{id}")]
        public async Task<ActionResult<ProfileDTO>> UpdateUserProfileByUserId(int id, ProfileDTO profileInfo)
        {
            try
            {
                return await _userService.UpdateProfileInfoByUserIdAsync(id, profileInfo);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}