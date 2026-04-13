using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string? Username {get; set;}
        public string? Email { get; set; }
        public string? UserIcon { get; set; }
    }
}