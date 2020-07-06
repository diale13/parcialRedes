using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;
using System.Collections.Generic;
using System.Threading;

namespace DataAccess
{
    public class AssociationHandler : IAsociationHandler
    {
        private ModifyQueue queue = ModifyQueue.GetInstance();
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private List<Genre> genres = MemoryDataBase.GetInstance().Genres;
        private List<Movie> movies = MemoryDataBase.GetInstance().Movies;
        private List<Director> directors = MemoryDataBase.GetInstance().Directors;

        public void UpdateMovieAndGenre(Movie mov, Genre gen)
        {
            var movieName = mov.Name;
            var genreName = gen.Name;
            try
            {
                semaphore.WaitAsync();
                queue.ChckAndAddToMovieList(movieName);
                queue.ChckAndAddToGenreList(genreName);

                UpdateGenreAfterAsociation(gen);
                UpdateMovieAfterASociation(mov);
            }
            catch (EntityBeingModifiedException entitiyBeingMod)
            {
                throw entitiyBeingMod;
            }
            catch (DataBaseException entityNotFound)
            {
                throw entityNotFound;
            }
            finally
            {
                RemoveMovieAndGenreFromQueue(genreName, movieName);
                semaphore.Release();
            }
        }


        private void UpdateMovieAfterASociation(Movie mov)
        {
            try
            {
                semaphore.WaitAsync();
                IMovieDataAccess movieDataAccess = new MovieDataAccess();
                movieDataAccess.Update(mov.Name, mov);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void UpdateGenreAfterAsociation(Genre gen)
        {
            try
            {
                semaphore.WaitAsync();
                IGenreDataAccess genreDataAccess = new GenreDataAccess();
                genreDataAccess.Update(gen.Name, gen);
            }
            finally
            {
                semaphore.Release();
            }

        }


        private void UpdateDirectorAfterAsociation(Director dir)
        {
            //var indexToModify = directors.FindIndex(dirInList => dirInList.Name.Equals(dir.Name));
            //if (indexToModify == -1)
            //{
            //    throw new DataBaseException("No se encontro el director solicitado");
            //}
            //directors[indexToModify] = dir;
            IDirectorDataAccess directorDataAccess = new DirectorDataAccess();
            directorDataAccess.AddDirector(dir);
        }



        private void RemoveDirectorAndMovieFromQueue(string movieName, string dirName)
        {
            queue.RemoveMovieFromModifyQueue(movieName);
            queue.RemoveDirectorFromQueue(dirName);
        }


        private void RemoveMovieAndGenreFromQueue(string genreName, string movieName)
        {
            queue.RemoveMovieFromModifyQueue(movieName);
            queue.RemoveGenreFromQueue(genreName);
        }


        public void UpdateMovieDirector(Movie movie, Director director)
        {
            var movieName = movie.Name;
            var dirName = director.Name;
            try
            {
                semaphore.WaitAsync();
                queue.ChckAndAddToMovieList(movieName);
                queue.ChckAndAddToDirectorList(dirName);

                UpdateMovieAfterASociation(movie);
                UpdateDirectorAfterAsociation(director);

            }
            catch (EntityBeingModifiedException entitiyBeingMod)
            {
                throw entitiyBeingMod;
            }
            catch (DataBaseException entityNotFound)
            {
                throw entityNotFound;
            }
            finally
            {
                RemoveDirectorAndMovieFromQueue(movieName, dirName);
                semaphore.Release();
            }

        }
    }
}
