using System.ComponentModel.DataAnnotations;

namespace ServerAPI.Models
{
    public class RemoveRatingModel
    {
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }
    }
}