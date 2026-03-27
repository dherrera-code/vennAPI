using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models.DTO
{
    public class ProfileDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? UserIcon { get; set; }
        public string? Banner { get; set; }
    }
}