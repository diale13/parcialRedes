using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataAccess
{
    public class GenreDataAccess : IGenreDataAccess
    {
        private List<Genre> genres = MemoryDataBase.GetInstance().Genres;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        ModifyQueue queue = ModifyQueue.GetInstance();

        public void Delete(Genre genreToDelete)
        {
            queue.ChckAndAddToGenreList(genreToDelete.Name);
            try
            {
                semaphore.WaitAsync();
                var indexToDelete = genres.FindIndex(gen => gen.Name.Equals(genreToDelete.Name));
                if (indexToDelete == -1)
                {
                    queue.RemoveGenreFromQueue(genreToDelete.Name);
                    throw new DataBaseException("No se encontro el genero solicitado");
                }

                genres.RemoveAt(indexToDelete);
            }
            finally
            {
                semaphore.Release();
                queue.RemoveGenreFromQueue(genreToDelete.Name);
            }
        }
        public List<Genre> GetGenres()
        {
            try
            {
                semaphore.WaitAsync();
                return genres;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public bool Exists(string genre)
        {
            return genres.FindIndex(gen => gen.Name.Equals(genre)) != -1;
        }

        public void Update(string genreName, Genre updatedGenre)
        {
            queue.ChckAndAddToGenreList(genreName);
            try
            {
                semaphore.WaitAsync();
                var indexToModify = genres.FindIndex(gen => gen.Name.Equals(genreName));
                if (indexToModify == -1)
                {
                    queue.RemoveGenreFromQueue(genreName);
                    throw new DataBaseException("No se encontro el genero solicitado");
                }
                genres[indexToModify] = updatedGenre;
            }
            finally
            {
                semaphore.Release();
                queue.RemoveGenreFromQueue(genreName);
            }
        }

        public void Upload(Genre genre)
        {
            try
            {
                semaphore.WaitAsync();

                if (IsNameUnique(genre.Name))
                {
                    genres.Add(genre);
                }
                else
                {
                    throw new DataBaseException("Ya existe un genero con ese nombre");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public Genre GetGenre(string name)
        {
            try
            {
                semaphore.WaitAsync();
                var indexToReturn = genres.FindIndex(gen => gen.Name.Equals(name));
                if (indexToReturn == -1)
                {
                    throw new DataBaseException("No se encontro el genero solicitado");
                }
                return genres[indexToReturn];
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool IsNameUnique(string name)
        {
            var validationInt = genres.FindIndex(gen => gen.Name.Equals(name));
            return (validationInt == -1);
        }
    }
}
