using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Genre
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public List<string> MoviesOfGenre { get; set; }


        public Genre()
        {

        }
    }
}
