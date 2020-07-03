using Common;
using Common.Interfaces;
using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Services;
using System;
using System.Collections.Generic;

namespace Server
{
    public static class ServerGenreManager
    {
        public static void UploadGenre(byte[] frame)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            Genre genre = new Genre
            {
                Name = data[0],
                Description = data[1],
                MoviesOfGenre = new List<string>()
            };
            genreService.Upload(genre);
        }

        public static void DeleteGenre(byte[] frame)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            var name = data[0];
            var genreToDelete = genreService.GetGenre(name);
            genreService.Delete(genreToDelete);

        }

        public static void ModifyGenre(byte[] frame)
        {
            IGenreDataAccess genreDataAccess = new GenreDataAccess();
            IGenreService genreService = new GenreService(genreDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);

            string oldName = data[0];

            var movieList = genreService.GetGenre(oldName).MoviesOfGenre;

            Genre genre = new Genre
            {
                Name = data[1],
                Description = data[2],
                MoviesOfGenre = movieList
            };
            genreService.Update(oldName, genre);
        }
    }
}
