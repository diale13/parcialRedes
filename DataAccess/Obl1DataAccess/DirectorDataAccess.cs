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
    public class DirectorDataAccess : IDirectorDataAccess
    {
        //private List<Director> directors = MemoryDataBase.GetInstance().Directors;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        //ModifyQueue queue = ModifyQueue.GetInstance();
        private Context context;

        public DirectorDataAccess()
        {
            context = new Context();
            context.Configuration.AutoDetectChangesEnabled = false;

        }

        public void AddDirector(Director director)
        {
            try
            {
                //semaphore.WaitAsync();
                if (!NameNotUnique(director.Name))
                {
                    //directors.Add(director);
                    context.Set<Director>().Add(director);
                    context.SaveChangesAsync();
                }
                else
                {
                    throw new DataBaseException("Ya existe un director con ese nombre");
                }
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo ingresar el director {director.Name} no pudo ser ingresado en la bd");
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool NameNotUnique(string name)
        {
            //var validationInt = directors.FindIndex(dir => dir.Name.Equals(name));
            //return (validationInt == -1);
            List<Director> directors = GetDirectors();
            bool exists = false;
            foreach (var director in directors)
                if (director.Name == name)
                    return true;
            return exists;
        }

        public void DeleteDirector(string directorName)
        {

            try
            {
                semaphore.WaitAsync();
                var directorToDelete = GetDirector(directorName);
                context.Directors.Remove(directorToDelete);
                context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudo borrar el director de la base de datos");
            }
            finally
            {
                semaphore.Release();

            }
        }

        public List<Director> GetDirectors()
        {
            try
            {
                semaphore.WaitAsync();
                List<Director> directors = context.Directors.ToList();
                List<Director> ret = new List<Director>();
                foreach (var director in directors)
                {
                    ret.Add(GetDirector(director.Name));
                }
                return ret;
            }

            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception e)
            {
                throw new DataBaseException("No se pudieron obtener los directores");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void UpdateDirector(string currentName, Director updatedDirector)
        {
            try
            {
                semaphore.WaitAsync();
                List<Director> directors = GetDirectors();
                if (directors.Count == 0)
                {
                    throw new DataBaseException("No se encontro el director solicitado");
                }
                DeleteDirector(currentName);
                AddDirector(updatedDirector);
                context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo acceder a la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudo actualizar el director");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public Director GetDirector(string name)
        {
            try
            {
                semaphore.WaitAsync();
                List<Director> directors = context.Directors.ToList();
                var directorSearched = new Director();
                foreach (var director in directors)
                    if (director.Name.Equals(name))
                        directorSearched = director;
                if (directorSearched == null)
                {
                    throw new DataBaseException("No se encontro el director solicitado");
                }
                IMovieDataAccess movieDataAccess = new MovieDataAccess();
                List<Movie> movies = movieDataAccess.GetMovies();
                foreach (var movie in movies)
                {
                    if (movie.Director.Equals(directorSearched.Name))
                        directorSearched.DirectedMovies.Add(movie.Name);
                }
                return directorSearched;
            }

            catch (DataBaseException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se encontro el director solicitado");
            }
            finally
            {
                semaphore.Release();
            }

        }
    }
}