using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MovieService : IMovieService
    {

        private readonly IMovieDataAccess movieDataA;

        public MovieService(IMovieDataAccess movieDataA)
        {
            this.movieDataA = movieDataA;
        }

        public void Delete(Movie mov)
        {
            movieDataA.Delete(mov);
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
            movieDataA.Upload(movieToUpload);
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
