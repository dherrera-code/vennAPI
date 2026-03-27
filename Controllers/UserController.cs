using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(UserServices userService) : ControllerBase
    {
        private readonly UserServices _userService = userService;

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]CreateAccountDTO newUser)
        {
            bool success = await _userService.CreateAccount(newUser);
            if(success) return Ok(new {success =  true, Message = "User Created!"});
            return BadRequest(new {Success = false, Message = "User Creation failed! Email is already in use or Username is already taken!"});
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userLogin)
        {
            var success = await _userService.Login(userLogin);

            if(success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login was unsuccessful"});
        }

        [HttpPost("DeleteUser/{userToDelete}")]
        public async Task<IActionResult> DeleteUser(string userToDelete)
        {
            var success = await _userService.DeleteUser(userToDelete);

            if(success) return Ok(new {Message = "Account Removed!"});

            return BadRequest(new {Message = "Unable to Remove Account!"});
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserInfoDTOByUsernameAsync(username);
            
            if(user != null) return Ok(user);

            return BadRequest(new {message = "No User Found"});
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<UserModel>> GetUser(int userId)
        {
            return await _userService.GetUserByUserIdAsync(userId);
        }

        [HttpPut("UpdateUsername/{id}/{newUsername}")]
        public async Task<IActionResult> UpdateUsername(int id, string newUsername)
        {
            var success = await _userService.EditUsername(id, newUsername);

            if(success) return Ok(new {success});

            return BadRequest(new {Message = "Updating Username Failed! New Username may already be created!"});
        }

        // In the future, I may move both profile endpoints to another controller! For Authentication purposes!

        [HttpGet("GetProfileByUserId/{id}")]
        public async Task<ActionResult<ProfileDTO>> GetUserProfileByUserId(int id)
        {
            return await _userService.GetUserProfileByIdAsync(id);
        }

        [HttpPut("UpdateUserProfileByUserId/{id}")]
        public async Task<ActionResult<ProfileDTO>> UpdateUserProfileByUserId(int id, ProfileDTO profileInfo)
        {
            return await _userService.UpdateProfileInfoByUserIdAsync(id, profileInfo);
        }
    }
}