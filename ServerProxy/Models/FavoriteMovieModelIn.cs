using System.ComponentModel.DataAnnotations;
namespace ServerProxy.Models
{
    public class FavoriteMovieModelIn
    {
        [Required]
        [StringLength(100)]
        public string MovieName { get; set; }
    }


}
