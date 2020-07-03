using DataAccess;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using Services.LogServices;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Services.RemotingServices
{
    public class MovieRemotingService : MarshalByRefObject, IMovieRemotingService
    {
        private IMovieDataAccess movieDataAcces = new MovieDataAccess();
        private IApiUsersDataAccess usersDataAccess = new ApiUsersDataAccess();
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private LoggerAssist loger = new LoggerAssist();
       


        public List<Movie> GetAllMovies()
        {
            return movieDataAcces.GetMovies();
        }

        public Movie GetMovie(string name)
        {
            try
            {
                return movieDataAcces.GetMovie(name);

            }
            catch (DataBaseException)
            {
                return null;
            }
        }

        public bool RemoveRating(string movieName, string nickName)
        {
            try
            {
                semaphore.WaitAsync();
                var movie = movieDataAcces.GetMovie(movieName);              
                if (movie.UserRating == null)
                {
                    return false;
                }
                if (movie.UserRating.ContainsKey(nickName))
                {
                    movie.UserRating.Remove(nickName);
                    movie.UpdateRating();
                    movieDataAcces.Update(movieName, movie);
                }             
                return true;
            }
            catch (DataBaseException)
            {
                return false;
            }
            finally
            {
                semaphore.Release();
                var action = $"{nickName} removed his rating of {movieName}";
                loger.EventCreator("DELRATING", action);
            }
        }

        public bool AddOrUpdateRating(string moviename, string nickName, int rating)
        {
            try
            {
                semaphore.WaitAsync();
                var movie = movieDataAcces.GetMovie(moviename);
                var user = usersDataAccess.Get(nickName);

                if (movie.UserRating == null)
                {
                    movie.UserRating = new Dictionary<string, int>();
                }
                if (movie.UserRating.ContainsKey(nickName))
                {
                    movie.UserRating[nickName] = rating;
                }
                movie.UserRating.Add(nickName, rating);
                movie.UpdateRating();
                movieDataAcces.Update(moviename, movie);

                var action = $"{nickName} voted {moviename} a {rating} out of 10!";
                loger.EventCreator("ADDRATING", action);

                return true;

            }
            catch (DataBaseException)
            {
                return false;
            }
            finally
            {
                semaphore.Release();
            }


        }


    }
}
