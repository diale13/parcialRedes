using Domain;
using System.Collections.Generic;

namespace IDataAccess
{
    public interface IMovieDataAccess
    {
        void Upload(Movie mov);
        void Update(string movie, Movie updatedMovie);
        void Delete(Movie mov);
        Movie GetMovie(string movieName);
        List<Movie> GetMovies();
    }
}
