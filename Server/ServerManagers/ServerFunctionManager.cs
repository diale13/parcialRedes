using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using DataAccess;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using IServices;
using Services;

namespace Server
{
    public static class ServerFunctionManager
    {
        public static void DisplayClients()
        {
            IClientDataAccess cliDa = new ClientDataAccess();
            IClientService cliS = new ClientService(cliDa);
            var clientList = cliS.GetConnectedClients();
            Console.WriteLine("Hay " + clientList.Count + " usuarios conectados");
            foreach (var client in clientList)
            {
                Console.WriteLine(client);
            }
        }

        public static void DisplayGenres()
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);
            var genreList = genreService.GetGenres();
            foreach (var genre in genreList)
            {
                Console.WriteLine("Nombre: " + genre.Name + "  Descripción: " + genre.Description +
                     "  Lista de películas de este género: ");
                var movieList = genre.MoviesOfGenre;
                foreach (var movie in movieList)
                {
                    Console.Write(movie);
                }

            }
        }

        public static void DisplayMoviesAddedOrder()
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);
            var movieList = movieService.GetMovies();
            foreach (var movie in movieList)
            {
                Console.WriteLine(movie);
            }
        }
        public static void DisplayMoviesByDate()
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movService = new MovieService(movieDataAccess);
            var allMovies = movService.GetMovies();
            var moviesByDate = allMovies.OrderBy(x => x.Date).ToList();
            foreach (var movie in moviesByDate)
            {
                Console.WriteLine(movie);
            }
        }
        public static void DisplayMoviesByDirector()
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movService = new MovieService(movieDataAccess);
            var allMovies = movService.GetMovies();
            var moviesByDate = allMovies.OrderBy(x => x.Director).ToList();
            foreach (var movie in moviesByDate)
            {
                Console.WriteLine(movie);
            }
        }

        public static void DisplayMoviesByGenre()
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movService = new MovieService(movieDataAccess);
            var allMovies = movService.GetMovies();

            List<string> uniqueGenresToListBy = new List<string>();

            foreach (var movie in allMovies)
            {
                foreach (var genre in movie.Genres)
                {
                    uniqueGenresToListBy.Add(genre);
                    uniqueGenresToListBy = uniqueGenresToListBy.Distinct().ToList();
                }
            }

            foreach (var genre in uniqueGenresToListBy)
            {
                Console.WriteLine("GENERO: " + genre);
                foreach (var movie in allMovies)
                {
                    if (movie.Genres.Contains(genre))
                    {
                        Console.WriteLine(movie);
                    }
                }
            }

        }

        public static void DisplayMovieFiles(string movieName)
        {
            IMovieDataAccess movieDataAccess = new MovieDataAccess();
            IMovieService movieService = new MovieService(movieDataAccess);

            IFileFunctions fileFunctions = new FileFunctions();
            try
            {
                var movie = movieService.GetMovie(movieName);

                string[] separator = { "@" };

                foreach (var file in movie.Files)
                {
                    var fileInfoSplitted = file.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    var fileSize = fileFunctions.GetFileSize(fileInfoSplitted[0]);
                    var fileName = fileFunctions.GetFileName(fileInfoSplitted[0]);

                    Console.WriteLine("Nombre: " + fileName + " Tamaño del archivo: " + fileSize + " Fecha de subida: " + fileInfoSplitted[1]);
                }
            }
            catch (DataBaseException)
            {
                Console.WriteLine("No se encontro la pelicula" + movieName);
                return;
            }
        }
    }
}
