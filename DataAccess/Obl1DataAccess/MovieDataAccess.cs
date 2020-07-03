using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;
using System.Collections.Generic;
using System.Threading;

namespace DataAccess
{
    public class MovieDataAccess : IMovieDataAccess
    {
        private List<Movie> movies = MemoryDataBase.GetInstance().Movies;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        ModifyQueue queue = ModifyQueue.GetInstance();

        public void Delete(Movie movie)
        {
            queue.ChckAndAddToMovieList(movie.Name);
            try
            {
                semaphore.WaitAsync();
                var indexToDelete = movies.FindIndex(mov => mov.Name.Equals(movie.Name));
                if (indexToDelete == -1)
                {
                    queue.RemoveMovieFromModifyQueue(movie.Name);
                    throw new DataBaseException("No se encontro la pelicula solicitada");
                }
                movies.RemoveAt(indexToDelete);
            }
            finally
            {
                semaphore.Release();
                queue.RemoveMovieFromModifyQueue(movie.Name);
            }
        }

        public Movie GetMovie(string movieName)
        {
            try
            {
                semaphore.WaitAsync();
                int indexToReturn = movies.FindIndex(mov => mov.Name.Equals(movieName));
                if (indexToReturn == -1)
                {
                    throw new DataBaseException("No se encontro la pelicula solicitada");
                }
                return movies[indexToReturn];
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Update(string movieName, Movie updatedMovie)
        {
            queue.ChckAndAddToMovieList(movieName);
            try
            {
                semaphore.WaitAsync();
                var indexToModify = movies.FindIndex(mov => mov.Name.Equals(movieName));
                if (indexToModify == -1)
                {
                    queue.RemoveMovieFromModifyQueue(movieName);
                    throw new DataBaseException("No se encontro la pelicula solicitada");
                }
                movies[indexToModify] = updatedMovie;
            }
            finally
            {
                semaphore.Release();
                queue.RemoveMovieFromModifyQueue(movieName);
            }
        }


        public void Upload(Movie mov)
        {
            try
            {
                semaphore.WaitAsync();
                var uniqueNameValidator = movies.FindIndex(movieInList => movieInList.Name.Equals(mov.Name));
                if (IsNameUnique(mov.Name))
                {
                    movies.Add(mov);
                }
                else
                {
                    throw new DataBaseException("Ya existe una pelicula con ese nombre");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool IsNameUnique(string name)
        {
            var validationInt = movies.FindIndex(movieInList => movieInList.Name.Equals(name));
            return (validationInt == -1);
        }

        public List<Movie> GetMovies()
        {
            try
            {
                semaphore.WaitAsync();
                return movies;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
