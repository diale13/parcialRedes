using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class MovieSimpleModelOUT
    {
        
        public string Name { get; set; }
        public List<string> Genres { get; set; }     
        public DateTime Date { get; set; }
        public string Director { get; set; }

        public MovieSimpleModelOUT(Movie mov)
        {
            this.Name = mov.Name;
            this.Genres = mov.Genres;
            this.Date = mov.Date;
            this.Director = mov.Director;
        }

    }
}