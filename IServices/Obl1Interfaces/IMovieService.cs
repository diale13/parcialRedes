using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IMovieService
    {
        void Upload(Movie mov);
        void Delete(Movie name);
        void Update(string oldName, Movie updatedMovie);
        Movie GetMovie(string name);
        List<Movie> GetMovies();
    }
}
