using Domain;
using System;
using System.Collections.Generic;

namespace ServerAdmin.Models
{
    public class MovieModel
    {

        public string Name { get; set; }
        public List<string> Genres { get; set; }
        public DateTime Date { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }


        public MovieModel(Movie mov)
        {
            Name = mov.Name;
            Director = mov.Director;
            Genres = mov.Genres;
            Date = mov.Date;
            Rating = mov.TotalRating;  
        }
    }
}