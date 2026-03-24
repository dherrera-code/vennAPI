
using System.ComponentModel.DataAnnotations;
namespace vennAPI.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public ICollection<RoomModel> RoomCreated {get; set;} = [];

    }
}