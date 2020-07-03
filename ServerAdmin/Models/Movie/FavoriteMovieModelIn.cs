using System.ComponentModel.DataAnnotations;

namespace ServerAdmin.Models
{
    public class FavoriteMovieModelIn
    {
        [Required]
        [StringLength(100)]
        public string MovieName { get; set; }    
    }
}