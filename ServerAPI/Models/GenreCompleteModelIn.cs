using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class GenreCompleteModelIn
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<string> MoviesOfGenre { get; set; }

        public Genre ToEntity()
        {
            var ret = new Genre()
            {
                Name = this.Name,
                Description = this.Description,
                MoviesOfGenre = this.MoviesOfGenre
            };
            if (ret.MoviesOfGenre == null)
            {
                ret.MoviesOfGenre = new List<string>();
            }
            return ret;
        }
    }
}