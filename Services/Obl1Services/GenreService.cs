using IDataAccess;
using Domain;
using IServices;
using System;
using System.Collections.Generic;

namespace Services
{
    public class GenreService : IGenreService
    {
        private IGenreDataAccess genreDataAccess;
        public GenreService(IGenreDataAccess genreDataAccess)
        {
            this.genreDataAccess = genreDataAccess;
        }

        public void Delete(Genre genreToDelete)
        {
            if (GenreIsAsociatedToMovie(genreToDelete))
            {
                throw new AsociatedClassException("No se pudo borrar el genero porque esta asociado a una pelicula");
            }
            genreDataAccess.Delete(genreToDelete);
        }

        public bool Exists(string genre)
        {
            return genreDataAccess.Exists(genre);
        }

        private bool GenreIsAsociatedToMovie(Genre genreToBeDeleted)
        {
            return genreToBeDeleted.MoviesOfGenre.Count > 0;
        }


        public void Update(string genreName, Genre updatedGenre)
        {
            genreDataAccess.Update(genreName, updatedGenre);
        }

        public void Upload(Genre genreToUpload)
        {
            genreDataAccess.Upload(genreToUpload);
        }

        public Genre GetGenre(string name)
        {
            return genreDataAccess.GetGenre(name);
        }

        public List<Genre> GetGenres()
        {
            return genreDataAccess.GetGenres();
        }

    }
}
