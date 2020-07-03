using Domain;
using System.Collections.Generic;

namespace IServices
{
    public interface IMovieRemotingService
    {
        List<Movie> GetAllMovies();
        Movie GetMovie(string name);
        bool AddOrUpdateRating(string moviename, string nickName, int rating);
        bool RemoveRating(string movieName, string nickName);
    }
}
