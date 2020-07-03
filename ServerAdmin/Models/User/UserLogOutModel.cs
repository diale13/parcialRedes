using System.ComponentModel.DataAnnotations;
namespace ServerAdmin.Models
{
    public class UserLogOutModel
    {
        [Required]
        public string Token { get; set; }
    }
}