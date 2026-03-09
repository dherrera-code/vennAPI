using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vennAPI.Models.DTO;
using vennAPI.Services;

namespace vennAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userService;
        public UserController(UserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]CreateAccountDTO newUser)
        {
            bool success = await _userService.CreateAccount(newUser);
            if(success) return Ok(new {success =  true, Message = "User Created!"});
            return BadRequest(new {Success = false, Message = "User Creation failed! Email is already in use!"});
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userLogin)
        {
            var success = await _userService.Login(userLogin);

            if(success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login was unsuccessful"});
        }
        
    }
}