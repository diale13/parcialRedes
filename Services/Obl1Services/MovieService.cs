using DataAccess;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Services
{
    public class MovieService : IMovieService
    {

        private readonly IMovieDataAccess movieDataA;
        private IDirectorService dirService;
        private IDirectorDataAccess dirDa = new DirectorDataAccess();

        public MovieService(IMovieDataAccess movieDataA)
        {
            this.movieDataA = movieDataA;
            dirService = new DirectorService(dirDa);

        }

        public void Delete(string mov)
        {
            var movieToDelete = movieDataA.GetMovie(mov);
            movieDataA.Delete(movieToDelete);
        }

        public void Update(string oldName, Movie updatedMovie)
        {
            if (updatedMovie.Genres.Count() < 1)
            {
                throw new BussinesLogicException("Una pelicula debe tener uno o mas generos asociados");
            }
            movieDataA.Update(oldName, updatedMovie);
        }

        public void Upload(Movie movieToUpload)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);



            if (movieToUpload.Genres.Count() < 1)
            {
                throw new BussinesLogicException("Una pelicula debe tener uno o mas generos asociados");
            }
            bool existsGenre = false;
            foreach (var genre in movieToUpload.Genres)
            {
                existsGenre = genreService.Exists(genre);
            }
            if (!existsGenre)
            {
                throw new BussinesLogicException("Ningun genero especificado coincide con el/los brindado/s");
            }
            Director dir = new Director();
            try
            {
                dir = dirDa.GetDirector(movieToUpload.Director);
                UpdateDirectorMovies(dir, movieToUpload.Name);
            }
            catch (DataBaseException)
            {
                throw new BussinesLogicException($"El director especificado no existe ");
            }
            movieDataA.Upload(movieToUpload);
            foreach (var gen in movieToUpload.Genres)
            {
                var genInList = genreService.GetGenre(gen);
                genInList.MoviesOfGenre.Add(movieToUpload.Name);
                genreService.Update(gen, genInList);
            }

        }

        private void UpdateDirectorMovies(Director dir, string movieName)
        {
            dir.DirectedMovies.Add(movieName);
            var dirMov = dir.DirectedMovies.Distinct().ToList();
            dir.DirectedMovies = dirMov;
            dirService.UpdateDirector(dir.Name, dir);
        }



        public Movie GetMovie(string name)
        {
            return movieDataA.GetMovie(name);
        }

        public List<Movie> GetMovies()
        {
            return movieDataA.GetMovies();
        }
    }
}
