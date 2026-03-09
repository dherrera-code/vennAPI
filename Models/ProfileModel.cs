using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vennAPI.Models
{
    public class ProfileModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        // public string? Banner { get; set; }
        public string? UserIcon { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; } //! Considering using geolocation instead!
    }
}