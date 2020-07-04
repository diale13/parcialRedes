using DataAccess;
using IDataAccess;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class AsociationApiService : IAsociationApiService
    {
        private IAsociationService asociationService;
        private IMovieDataAccess movieDataAccess;
        private IGenreDataAccess genreDataAccess;
        private IDirectorDataAccess directorDataAccess;


        public AsociationApiService()
        {
            IAsociationHandler asociationHandler = new AssociationHandler();
            asociationService = new AsociationService(asociationHandler);
            movieDataAccess = new MovieDataAccess();
            genreDataAccess = new GenreDataAccess();
            directorDataAccess = new DirectorDataAccess();
        }


        public void DeAsociateGenreMovie(string movieName, string genreName)
        {
            var mov = movieDataAccess.GetMovie(movieName);
            var gen = genreDataAccess.GetGenre(genreName);
            asociationService.DeAsociateGenreMovie(mov, gen);
        }

        public void AsociateGenreToMovie(string movieName, string genreName)
        {
            var mov = movieDataAccess.GetMovie(movieName);
            var gen = genreDataAccess.GetGenre(genreName);
            asociationService.AsociateGenreToMovie(mov, gen);
        }


        public void AsociateDirectorMovie(string movieName, string directorName)
        {
            var mov = movieDataAccess.GetMovie(movieName);
            var dir = directorDataAccess.GetDirector(directorName);
            asociationService.AsociateDirectorMovie(mov, dir);
        }


        public void DeAsociatDirMovie(string movieName, string directorName)
        {
            var mov = movieDataAccess.GetMovie(movieName);
            var dir = directorDataAccess.GetDirector(directorName);
            asociationService.DeAsociatDirMovie(mov, dir);

        }




    }
}
