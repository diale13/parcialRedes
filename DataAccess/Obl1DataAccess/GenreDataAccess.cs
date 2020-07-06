using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace DataAccess
{
    public class GenreDataAccess : IGenreDataAccess
    {
        //private List<Genre> genres = MemoryDataBase.GetInstance().Genres;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        //ModifyQueue queue = ModifyQueue.GetInstance();

        private Context context;

        public GenreDataAccess()
        {
            context = new Context();
            context.Configuration.AutoDetectChangesEnabled = false;
        }

        public void Delete(Genre genreToDelete)
        {
            try
            {
                semaphore.WaitAsync();
                if (Exists(genreToDelete.Name))
                {
                    context.Genres.Remove(genreToDelete);
                    List<MovieGenreAssociation> movieGenreAssociations = context.MovieGenreAssociations.ToList();
                    foreach (var association in movieGenreAssociations)
                    {
                        if (association.GenreName.Equals(genreToDelete.Name))
                        {
                            context.MovieGenreAssociations.Remove(association);
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new DataBaseException($"No se encontró el género {genreToDelete.Name} en la base de datos");
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo borrar el género {genreToDelete.Name}");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public List<Genre> GetGenres()
        {
            try
            {
                semaphore.WaitAsync();
                if (context.Genres.ToList() == null)
                    return new List<Genre>();
                return context.Genres.ToList();
            }
            catch (DataBaseException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudo obtener los géneros de la base de datos");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public bool Exists(string genre)
        {
            List<Genre> genres = GetGenres();
            bool exists = false;
            foreach (var gen in genres)
                if (gen.Name == genre)
                    return true;
            return exists;
        }

        public void Update(string genreName, Genre updatedGenre)
        {
            try
            {
                semaphore.WaitAsync();
                var genre = context.Genres.SingleOrDefault(g => g.Name.Equals(genreName));
                if (genre != null)
                {
                    genre.Description = updatedGenre.Description;
                    List<MovieGenreAssociation> movieGenreAssociations = context.MovieGenreAssociations.ToList();
                    foreach (var association in movieGenreAssociations)
                    {
                        if (association.GenreName.Equals(genreName))
                        {
                            context.MovieGenreAssociations.Remove(association);
                            context.SaveChanges();
                        }
                    }
                    foreach (var movie in updatedGenre.MoviesOfGenre)
                    {
                        MovieGenreAssociation movieGenreAssociation = new MovieGenreAssociation()
                        {
                            GenreName = updatedGenre.Name,
                            MovieName = movie,
                        };
                        context.MovieGenreAssociations.Add(movieGenreAssociation);
                    }
                    context.SaveChanges();
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception e)
            {
                throw new DataBaseException($"No se pudo actualizar el género {genreName}");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Upload(Genre genre)
        {
            try
            {
                semaphore.WaitAsync();
                if (!Exists(genre.Name))
                {
                    context.Genres.Add(genre);
                    foreach (var movie in genre.MoviesOfGenre)
                    {
                        MovieGenreAssociation movieGenreAssociation = new MovieGenreAssociation()
                        {
                            GenreName = genre.Name,
                            MovieName = movie,
                        };
                        context.MovieGenreAssociations.Add(movieGenreAssociation);
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new DataBaseException("Ya existe un genero con ese nombre");
                }
            }
            catch (DbException e)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception e)
            {
                throw new DataBaseException($"No se pudo agregar a la base de datos el género {genre.Name}");
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
                if (Exists(name))
                {
                    List<Genre> genres = GetGenres();
                    Genre genre = genres.Find(g => g.Name.Equals(name));
                    List<MovieGenreAssociation> movieGenreAssociations = context.MovieGenreAssociations.ToList();
                    foreach (var association in movieGenreAssociations)
                    {
                        if (association.GenreName.Equals(genre.Name))
                        {
                            genre.MoviesOfGenre.Add(association.MovieName);
                        }
                    }
                    return genre;
                }
                else
                {
                    throw new DataBaseException($"No existe ningun género con el nombre {name}");
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudo encontrar el elemento buscado");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
