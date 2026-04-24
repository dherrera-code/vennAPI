
using System.ComponentModel.DataAnnotations;
namespace vennAPI.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
        // * New columns for profile!
        public string Description { get; set; } = "";
        public string? UserIcon { get; set; }
        public string? Banner { get; set; }
        // ! End of profile columns
        
        public ICollection<RoomModel> RoomCreated {get; set;} = [];

        public ICollection<UserAvailability> Availability {get; set;} = [];

    }
}