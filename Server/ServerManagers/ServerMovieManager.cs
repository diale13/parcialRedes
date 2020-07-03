using Common;
using Common.Interfaces;
using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;

namespace Server
{
    public static class ServerMovieManager
    {
        public static void Upload(byte[] frame)
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IGenreDataAccess genreData = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreData);

            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            Movie movie = new Movie
            {
                Name = data[0],
                Date = DateTime.ParseExact(data[1], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Director = data[2],
                Files = new List<string>(),
            };
            try
            {
                movie.Genres = parser.GetList(data[3]);
            }
            catch (IndexOutOfRangeException)
            {
                movie.Genres = new List<string>();
            }

            movieService.Upload(movie);

            //After uploading makes the movie and genre list consistent
            foreach (var genreInMovie in movie.Genres)
            {
                var genreThatNeedsToBeAsociatedToMovie = genreService.GetGenre(genreInMovie);
                genreThatNeedsToBeAsociatedToMovie.MoviesOfGenre.Add(movie.Name);
                genreService.Update(genreThatNeedsToBeAsociatedToMovie.Name, genreThatNeedsToBeAsociatedToMovie);
            }

        }

        public static void Delete(byte[] frame)
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IGenreDataAccess genreData = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreData);

            IAsociationHandler asociationHandler = new AssociationHandler();
            IAsociationService asociationService = new AsociationService(asociationHandler);

            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            var name = data[0];
            var movieToDelete = movieService.GetMovie(name);

            foreach (var genreName in movieToDelete.Genres)
            {
                var genreToDesaciate = genreService.GetGenre(genreName);
                asociationService.DeAsociateGenreMovie(movieToDelete, genreToDesaciate);
            }

            movieService.Delete(movieToDelete);
        }

        public static void Modify(byte[] frame)
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);

            string oldName = data[0];

            var genreList = movieService.GetMovie(oldName).Genres;
            Movie movie = new Movie
            {
                Name = data[1],
                Date = DateTime.ParseExact(data[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Director = data[3],
                Genres = genreList
            };

            movieService.Update(oldName, movie);
        }

        public static string SaveFile(byte[] frame, NetworkStream networkStream)
        {
            IFileFunctions fileFunctions = new FileFunctions();
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);
            IFileHandler fileHandler = new FileHandler(networkStream);
            IFileService fileService = new FileService(fileFunctions, fileHandler);

            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);

            Movie referredMovie = movieService.GetMovie(data[0]);

            DateTime date = DateTime.Now;

            referredMovie.Files.Add(data[1] + "@" + date);

            string oldName = data[0];
            Movie movie = new Movie
            {
                Name = referredMovie.Name,
                Date = referredMovie.Date,
                Director = referredMovie.Director,
                Files = referredMovie.Files,
                Genres = referredMovie.Genres
            };

            movieService.Update(oldName, movie);

            return data[1];
        }

        public static void ReceiveFile(NetworkStream networkStream)
        {
            IFileFunctions fileFunctions = new FileFunctions();
            IFileHandler fileHandler = new FileHandler(networkStream);
            IFileService fileService = new FileService(fileFunctions, fileHandler);
            fileService.ReceiveFile();
        }

    }

}

