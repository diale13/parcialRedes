using Domain;
using System.Collections.Generic;

namespace IServices
{
    public interface IApiUserService
    {
        bool AddUser(ApiUser user);
        bool UpdateUser(ApiUser user);
        ApiUser GetUser(string nickName);
        bool DeleteUser(string nicknName, string password);
        List<Movie> GetFavMovies(string nickName);
        bool AddFavoriteMovie(string nickName, string movieName);
        bool RemoveFavoriteMovie(string nickName,string movieName);
    }
}
