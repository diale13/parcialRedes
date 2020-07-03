using System.ComponentModel.DataAnnotations;

namespace ServerAdmin.Models.Rating
{
    public class RemoveRatingModel
    {
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }
    }
}