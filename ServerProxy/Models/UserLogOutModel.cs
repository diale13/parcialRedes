using System.ComponentModel.DataAnnotations;
namespace ServerProxy.Models
{
    public class UserLogOutModel
    {
        [Required]
        public string Token { get; set; }
    }
}