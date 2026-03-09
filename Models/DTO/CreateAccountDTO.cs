using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class CreateAccountDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
    }
}