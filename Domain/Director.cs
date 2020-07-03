using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Director
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> DirectedMovies { get; set; }
        public string Description { get; set; }


        public Director()
        {
            this.Name = "N/A";
        }

    }
}
