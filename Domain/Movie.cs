using System;
using System.Collections.Generic;

namespace Domain
{
    [Serializable]
    public class Movie
    {
        public string Name { get; set; }
        public List<string> Genres { get; set; }
        public DateTime Date { get; set; }
        public string Director { get; set; }
        public List<string> Files { get; set; }
        public Dictionary<string, int> UserRating { get; set; }
        public int TotalRating { get; set; }

        public void UpdateRating()
        {
            int total = 0;
            foreach (var item in UserRating)
            {
                total += item.Value;
            }
            total = (total / UserRating.Count);
            TotalRating = total;
        }

        public override string ToString()
        {
            string ret = "Movie: " + Name + " Premiere: " + Date.Day + "/" + Date.Month + "/" + Date.Year + " Directed by: " + Director + " Genres asociated: ";
            foreach (var gen in Genres)
            {
                ret += gen + ", ";
            }
            return ret;
        }

    }
}
