using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using System;
using System.Threading;

namespace Services
{
    public class AsociationService : IAsociationService
    {

        private readonly IAsociationHandler associationHandler;

        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public AsociationService(IAsociationHandler aHandler)
        {
            this.associationHandler = aHandler;
        }


        public void DeAsociateGenreMovie(Movie movie, Genre genre)
        {
            try
            {
                semaphore.WaitAsync();
                if (!movie.Genres.Contains(genre.Name))
                {
                    throw new BussinesLogicException("La pelicula no tiene el genero para ser desasociado");
                }

                movie.Genres.Remove(genre.Name);
                genre.MoviesOfGenre.Remove(movie.Name);

                associationHandler.UpdateMovieAndGenre(movie, genre);
            }
            finally
            {
                semaphore.Release();
            }

        }

        public void AsociateGenreToMovie(Movie movie, Genre genre)
        {
            semaphore.WaitAsync();
            try
            {
                if (movie.Genres.Contains(genre.Name))
                {
                    throw new BussinesLogicException("La pelicula ya tiene el genero");
                }

                movie.Genres.Add(genre.Name);
                genre.MoviesOfGenre.Add(movie.Name);

                associationHandler.UpdateMovieAndGenre(movie, genre);
            }
            finally
            {
                semaphore.Release();
            }
        }


        public void AsociateDirectorMovie(Movie movie, Director director)
        {            
            try
            {
                semaphore.WaitAsync();
                if (movie.Director.Equals(director))
                {
                    throw new BussinesLogicException("La pelicula ya tiene el director");
                }
                movie.Director = director.Name;
                director.DirectedMovies.Add(movie.Name);

                associationHandler.UpdateMovieDirector(movie, director);
            }
            finally
            {
                semaphore.Release();
            }
        }


        public void DeAsociatDirMovie(Movie movie, Director director)
        {
            try
            {
                semaphore.WaitAsync();
                if (!movie.Director.Equals(director.Name))
                {
                    throw new BussinesLogicException("El director no dirige esa pelicula");
                }

                movie.Director = "";
                director.DirectedMovies.Remove(movie.Name);

                associationHandler.UpdateMovieDirector(movie, director);
            }
            finally
            {
                semaphore.Release();
            }
        }


    }
}
