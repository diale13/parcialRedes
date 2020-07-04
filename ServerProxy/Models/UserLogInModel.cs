using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServerProxy.Models
{
    public class UserLogInModel
    {
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
                
    }
}