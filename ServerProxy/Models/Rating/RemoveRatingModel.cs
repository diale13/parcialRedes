using System.ComponentModel.DataAnnotations;

namespace ServerProxy.Models
{
    public class RemoveRatingModel
    {
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }
    }
}