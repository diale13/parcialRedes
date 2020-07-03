using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using System;

namespace Services
{
    public class AsociationService : IAsociationService
    {

        private readonly IAsociationHandler associationHandler;

        private readonly Object asociationLock;

        public AsociationService(IAsociationHandler aHandler)
        {
            this.associationHandler = aHandler;
            this.asociationLock = new object();
        }


        public void DeAsociateGenreMovie(Movie movie, Genre genre)
        {
            lock (asociationLock)
            {
                if (!movie.Genres.Contains(genre.Name))
                {
                    throw new BussinesLogicException("La pelicula no tiene el genero para ser desasociado");
                }

                movie.Genres.Remove(genre.Name);
                genre.MoviesOfGenre.Remove(movie.Name);

                associationHandler.UpdateMovieAndGenre(movie, genre);
            }

        }

        public void AsociateGenreToMovie(Movie movie, Genre genre)
        {
            lock (asociationLock)
            {
                if (movie.Genres.Contains(genre.Name))
                {
                    throw new BussinesLogicException("La pelicula ya tiene el genero");
                }

                movie.Genres.Add(genre.Name);
                genre.MoviesOfGenre.Add(movie.Name);

                associationHandler.UpdateMovieAndGenre(movie, genre);
            }
        }


        public void AsociateDirectorMovie(Movie movie, Director director)
        {
            lock (asociationLock)
            {
                if (movie.Director.Equals(director))
                {
                    throw new BussinesLogicException("La pelicula ya tiene el director");
                }
                movie.Director = director.Name;
                director.DirectedMovies.Add(movie.Name);

                associationHandler.UpdateMovieDirector(movie, director);
            }
        }


        public void DeAsociatDirMovie(Movie movie, Director director)
        {
            lock (asociationLock)
            {
                if (!movie.Director.Equals(director.Name))
                {
                    throw new BussinesLogicException("El director no dirige esa pelicula");
                }

                movie.Director = "";
                director.DirectedMovies.Remove(movie.Name);

                associationHandler.UpdateMovieDirector(movie, director);
            }

        }


    }
}
