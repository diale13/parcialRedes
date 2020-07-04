using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerAPI.Models
{
    public class MovieFullModel
    {
        [Required]
        public string Name { get; set; }
        public List<string> Genres { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Director { get; set; }

        //public List<string> Files { get; set; }        
        public int TotalRating { get; set; }

        public Movie ToEntity()
        {
            var movie = new Movie()
            {
                Name = this.Name,
                Genres = this.Genres,
                Date = this.Date,
                Director = this.Director,
                TotalRating = this.TotalRating
            };
            if (movie.Genres == null)
            {
                movie.Genres = new List<string>();
            }

            return movie;
        }



    }
}