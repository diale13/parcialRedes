using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Services;


namespace Server
{
    public static class ServerAsociationManager
    {
        public static void AsociateGenreToMovie(byte[] frame)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);

            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IAsociationHandler asociationHandler = new AssociationHandler();
            IAsociationService asociationService = new AsociationService(asociationHandler);

            IParser parser = new Parser();


            var movieAndGenreNames = parser.GetDataObject(frame);
            var movieToAsociate = movieService.GetMovie(movieAndGenreNames[0]);
            var genreToAsociate = genreService.GetGenre(movieAndGenreNames[1]);

            asociationService.AsociateGenreToMovie(movieToAsociate, genreToAsociate);
        }

        public static void DeAsociateGenreToMovie(byte[] frame)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);

            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IAsociationHandler asociationHandler = new AssociationHandler();
            IAsociationService asociationService = new AsociationService(asociationHandler);

            IParser parser = new Parser();


            var movieAndGenreNames = parser.GetDataObject(frame);
            var movieToAsociate = movieService.GetMovie(movieAndGenreNames[0]);
            var genreToAsociate = genreService.GetGenre(movieAndGenreNames[1]);

            asociationService.DeAsociateGenreMovie(movieToAsociate, genreToAsociate);
        }

        public static void AsociateDirectorToMovie(byte[] frame)
        {
            IDirectorDataAccess dirDa = new DirectorDataAccess();
            IDirectorService directorService = new DirectorService(dirDa);

            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IAsociationHandler asociationHandler = new AssociationHandler();
            IAsociationService asociationService = new AsociationService(asociationHandler);

            IParser parser = new Parser();

            var directorAndMovieNames = parser.GetDataObject(frame);
            var movieToAsociate = movieService.GetMovie(directorAndMovieNames[0]);
            var directorToASociate = directorService.GetDirector(directorAndMovieNames[1]);

            asociationService.AsociateDirectorMovie(movieToAsociate, directorToASociate);
        }

        public static void DeAsociateDirectorToMovie(byte[] frame)
        {
            IDirectorDataAccess dirDa = new DirectorDataAccess();
            IDirectorService directorService = new DirectorService(dirDa);

            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IAsociationHandler asociationHandler = new AssociationHandler();
            IAsociationService asociationService = new AsociationService(asociationHandler);

            IParser parser = new Parser();

            var directorAndMovieNames = parser.GetDataObject(frame);
            var movieToAsociate = movieService.GetMovie(directorAndMovieNames[0]);
            var directorToASociate = directorService.GetDirector(directorAndMovieNames[1]);

            asociationService.DeAsociatDirMovie(movieToAsociate, directorToASociate);
        }

    }
}
