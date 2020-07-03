using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerAPI
{
    public class DirectorCompleteModelIN
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Genre { get; set; }
        public List<string> DirectedMovies { get; set; }
        public string Description { get; set; }

        public Director ToEntity()
        {
            var ret = new Director
            {
                Name = this.Name,
                BirthDate = this.BirthDate,
                DirectedMovies = this.DirectedMovies,
                Genre = this.Genre,
                Description = Description
            };
            if(ret.DirectedMovies == null)
            {
                ret.DirectedMovies = new List<string>();
            }
            return ret;
        }

    }
}