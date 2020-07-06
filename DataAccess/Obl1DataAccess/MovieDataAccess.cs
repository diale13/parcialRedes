using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Microsoft.SqlServer.Server;
using Persistance;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataAccess
{
    public class MovieDataAccess : IMovieDataAccess
    {
        //private List<Movie> movies = MemoryDataBase.GetInstance().Movies;
        //private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        //ModifyQueue queue = ModifyQueue.GetInstance();

        private Context context;

        public MovieDataAccess()
        {
            context = new Context();
        }

        private void RemoveMovieFromDirectorList(Movie movie)
        {
            IDirectorDataAccess directorDataAccess = new DirectorDataAccess();
            List<Director> directors = directorDataAccess.GetDirectors();
            Director directorToModify = new Director();
            foreach (var director in directors)
            {
                if (director.Name.Equals(movie.Director))
                {
                    foreach (var actualMovie in director.DirectedMovies)
                    {
                        if (actualMovie.Equals(movie.Name))
                        {
                            director.DirectedMovies.Remove(actualMovie);
                            directorToModify = director;
                        }
                    }
                }
            }
            directorDataAccess.UpdateDirector(directorToModify.Name, directorToModify);
        }

        private void RemoveRating(Movie movie)
        {
            List<MovieUserAssociation> movieUserAssociations = context.MovieUserAssociations.ToList();
            foreach (var association in movieUserAssociations)
            {
                if (association.Movie.Equals(movie))
                {
                    context.MovieUserAssociations.Remove(association);
                    context.SaveChangesAsync();
                }
            }
        }

        public void Delete(Movie movie)
        {
            //queue.ChckAndAddToMovieList(movie.Name);
            //try
            //{
            //    semaphore.WaitAsync();
            //    var indexToDelete = movies.FindIndex(mov => mov.Name.Equals(movie.Name));
            //    if (indexToDelete == -1)
            //    {
            //        queue.RemoveMovieFromModifyQueue(movie.Name);
            //        throw new DataBaseException("No se encontro la pelicula solicitada");
            //    }
            //    movies.RemoveAt(indexToDelete);
            //}
            //finally
            //{
            //    semaphore.Release();
            //    queue.RemoveMovieFromModifyQueue(movie.Name);
            //}
            try
            {
                if (Exists(movie))
                {

                    context.Movies.Remove(movie);
                    RemoveMovieFromDirectorList(movie);
                    RemoveFiles(movie);
                    RemoveRating(movie);
                     context.SaveChangesAsync();
                }
                else
                {
                    throw new DataBaseException($"La película {movie.Name} no existe en la base de datos");
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"La película {movie.Name} no se pudo borrar");
            }
        }

        private void AddFiles(Movie movie)
        {
            foreach (var filePath in movie.Files)
            {
                FileMovie file = new FileMovie()
                {
                    MovieName = movie.Name,
                    Path = filePath,
                };
                context.FileMovies.Add(file);
            }
        }

        private void RemoveFiles(Movie movie)
        {
            List<FileMovie> files = context.FileMovies.ToList();
            foreach (var file in files)
            {
                if (file.MovieName.Equals(movie.Name))
                {
                    context.FileMovies.Remove(file);
                }
            }
        }

        public bool Exists(Movie movie)
        {
            List<Movie> movies = GetMovies();
            foreach (var mov in movies)
                if (mov.Name.Equals(movie))
                    return true;
            return false;
        }

        public Movie GetMovie(string movieName)
        {
            try
            {
                List<Movie> movies = GetMovies();
                foreach (var movie in movies)
                {
                    if (movie.Name.Equals(movieName))
                    {
                        List<MovieUserAssociation> movieUserAssociations = context.MovieUserAssociations.ToList();
                        foreach (var association in movieUserAssociations)
                        {
                            if (association.Movie.Equals(movieName))
                            {
                                movie.UserRating.Add(association.User, association.Rating);
                            }
                        }
                        List<MovieGenreAssociation> movieGenreAssociations = context.MovieGenreAssociations.ToList();
                        foreach (var association in movieGenreAssociations)
                        {
                            if (association.MovieName.Equals(movieName))
                            {
                                movie.Genres.Add(association.GenreName);
                            }
                        }
                        List<FileMovie> fileMovies = context.FileMovies.ToList();
                        foreach (var association in fileMovies)
                        {
                            if (association.MovieName.Equals(movieName))
                            {
                                movie.Files.Add(association.Path);
                            }
                        }
                        return movie;
                    }
                }
                throw new DataBaseException($"La película {movieName} no se encontró en la base de datos");
                //semaphore.WaitAsync();
                //int indexToReturn = movies.FindIndex(mov => mov.Name.Equals(movieName));
                //if (indexToReturn == -1)
                //{
                //    throw new DataBaseException("No se encontro la pelicula solicitada");
                //}
                //return movies[indexToReturn];
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo obtener la película {movieName}");
            }
        }

        public void AddMovieToDirectorMovieList(Movie movie)
        {
            IDirectorDataAccess directorDataAccess = new DirectorDataAccess();
            List<Director> directors = directorDataAccess.GetDirectors();
            Director directorToModify = new Director();
            foreach (var director in directors)
            {
                if (director.Name.Equals(movie.Director))
                {
                    director.DirectedMovies.Add(movie.Name);
                }
            }
            directorDataAccess.UpdateDirector(directorToModify.Name, directorToModify);
        }

        public void Update(string movieName, Movie updatedMovie)
        {
            try
            {
                var movie = context.Movies.SingleOrDefault(m => m.Name.Equals(movieName));
                if (movie != null)
                {
                    RemoveMovieFromDirectorList(movie);
                    RemoveFiles(movie);
                    RemoveRating(movie);
                    foreach (var association in updatedMovie.UserRating)
                    {
                        MovieUserAssociation movieUserAssociation = new MovieUserAssociation()
                        {
                            Movie = updatedMovie.Name,
                            Rating = association.Value,
                            User = association.Key,
                        };
                        context.MovieUserAssociations.Add(movieUserAssociation);
                    }
                    AddFiles(updatedMovie);
                    AddMovieToDirectorMovieList(updatedMovie);
                    movie.Date = updatedMovie.Date;
                    movie.Director = updatedMovie.Director;
                    context.SaveChangesAsync();
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo actualizar la película {movieName}");
            }
            //queue.ChckAndAddToMovieList(movieName);
            //try
            //{
            //    semaphore.WaitAsync();
            //    var indexToModify = movies.FindIndex(mov => mov.Name.Equals(movieName));
            //    if (indexToModify == -1)
            //    {
            //        queue.RemoveMovieFromModifyQueue(movieName);
            //        throw new DataBaseException("No se encontro la pelicula solicitada");
            //    }
            //    movies[indexToModify] = updatedMovie;
            //}
            //finally
            //{
            //    semaphore.Release();
            //    queue.RemoveMovieFromModifyQueue(movieName);
            //}
        }


        public void Upload(Movie mov)
        {
            try
            {
                if (!Exists(mov))
                {
                    foreach (var association in mov.UserRating)
                    {
                        MovieUserAssociation movieUserAssociation = new MovieUserAssociation()
                        {
                            Movie = mov.Name,
                            Rating = association.Value,
                            User = association.Key,
                        };
                        context.MovieUserAssociations.Add(movieUserAssociation);
                    }
                    AddMovieToDirectorMovieList(mov);
                    AddFiles(mov);
                    context.Movies.Add(mov);
                    context.SaveChangesAsync();
                }
                else
                {
                    throw new DataBaseException("Ya existe una pelicula con ese nombre");
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo cargar correctamente la pelicula {mov.Name}");
            }
        }

        public List<Movie> GetMovies()
        {
            try
            {
                List<Movie> movies = context.Movies.ToList();
                List<Movie> ret = new List<Movie>();
                foreach (var mov in movies)
                {
                    Movie movieToAdd = GetMovie(mov.Name);
                    ret.Add(movieToAdd);
                }
                return ret;
            }
            catch (DataBaseException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudieron obtener las películas de la base de datos");
            }
        }
    }
}
