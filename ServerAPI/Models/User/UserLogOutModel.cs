using System.ComponentModel.DataAnnotations;
namespace ServerAPI.Models
{
    public class UserLogOutModel
    {
        [Required]
        public string Token { get; set; }
    }
}