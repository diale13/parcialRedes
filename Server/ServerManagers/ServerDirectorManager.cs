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

namespace Server
{
    public static class ServerDirectorManager
    {
        public static void UploadDirector(byte[] frame)
        {
            IDirectorDataAccess directorDA = new DirectorDataAccess();
            IDirectorService directorService = new DirectorService(directorDA);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);

            Director dir = new Director
            {
                Name = data[0],
                BirthDate = DateTime.ParseExact(data[1], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Description = data[2],
                Genre = data[3],
                DirectedMovies = new List<string>()
            };
            directorService.AddDirector(dir);
        }

        public static void DeleteDirector(byte[] frame)
        {
            IDirectorDataAccess directorDA = new DirectorDataAccess();
            IDirectorService directorService = new DirectorService(directorDA);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            var dirName = data[0];
            var dirToDelete = directorService.GetDirector(dirName);
            directorService.DeleteDirector(dirToDelete);
        }

        public static void ModifyDirector(byte[] frame)
        {
            IDirectorDataAccess directorDA = new DirectorDataAccess();
            IDirectorService directorService = new DirectorService(directorDA);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);

            string oldName = data[0];

            var movieList = directorService.GetDirector(oldName).DirectedMovies;

            Director dir = new Director
            {
                Name = data[1],
                BirthDate = DateTime.ParseExact(data[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Description = data[3],
                DirectedMovies = movieList,
                Genre = data[4]
            };
            directorService.UpdateDirector(oldName, dir);
        }


    }
}
