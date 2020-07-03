using DataAccess;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using Services.LogServices;
using System;
using System.Collections.Generic;

namespace Services
{
    public class ApiUserService : MarshalByRefObject, IApiUserService
    {
        private IApiUsersDataAccess usersDataAccess = new ApiUsersDataAccess();
        private IMovieDataAccess movieDataAccess = new MovieDataAccess();
        private LoggerAssist loger = new LoggerAssist();       

        public bool AddUser(ApiUser user)
        {
            try
            {
                usersDataAccess.Add(user);
                var action = $"user {user.NickName} created";
                loger.EventCreator("POSTUSER", action);
                return true;
            }
            catch (DataBaseException)
            {
                return false;
            }

        }

        public bool DeleteUser(string nicknName, string password)
        {
            try
            {
                usersDataAccess.Delete(nicknName, password);
                var action = $"user {nicknName} deleted";
                loger.EventCreator("DELETEUSER", action);
                return true;
            }
            catch (DataBaseException)
            {
                return false;
            }

        }

        public ApiUser GetUser(string nickName)
        {
            var action = $"user {nickName} accesed";
            loger.EventCreator("GETUSER", action);
            return usersDataAccess.Get(nickName);
        }

        public bool UpdateUser(ApiUser user)
        {
            try
            {
                var action = $"user {user} updated";
                loger.EventCreator("PUTUSER", action);
                usersDataAccess.Update(user);
                return true;
            }
            catch (DataBaseException)
            {
                return false;
            }

        }

        public bool AddFavoriteMovie(string nickName, string movieName)
        {
            try
            {
                var movieInDb = movieDataAccess.GetMovie(movieName);
                var userInDb = usersDataAccess.Get(nickName);
                            

                if (!userInDb.FavoriteMovies.Contains(movieName))
                {
                    userInDb.FavoriteMovies.Add(movieName);
                }

                var action = $"{nickName} added {movieName} to his favorites";
                loger.EventCreator("ADDFAV", action);

                return true;

            }
            catch (DataBaseException)
            {
                return false;
            }

        }

        public bool RemoveFavoriteMovie(string nickName, string movieName)
        {
            var user = usersDataAccess.Get(nickName);
            if (user == null)
            {
                return false;
            }
            user.FavoriteMovies.Remove(movieName);


            var action = $"{nickName} removed {movieName} from his favorites";
            loger.EventCreator("DELFAV", action);

            return true;
        }

        public List<Movie> GetFavMovies(string nickName)
        {
            var user = usersDataAccess.Get(nickName);
            var favMovieNames = user.FavoriteMovies;
            var listOfActualMovies = new List<Movie>();
            foreach (var name in favMovieNames)
            {
                var movie = movieDataAccess.GetMovie(name);
                listOfActualMovies.Add(movie);
            }

            var action = $"{nickName} solicited his favorites movies";
            loger.EventCreator("GETFAV", action);

            return listOfActualMovies;
        }

    }
}
