using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;
using System.Collections.Generic;
using System.Threading;

namespace DataAccess
{
    public class DirectorDataAccess : IDirectorDataAccess
    {
        private List<Director> directors = MemoryDataBase.GetInstance().Directors;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        ModifyQueue queue = ModifyQueue.GetInstance();

        public void AddDirector(Director director)
        {
            try
            {
                semaphore.WaitAsync();
                if (IsNameUnique(director.Name))
                {
                    directors.Add(director);
                }
                else
                {
                    throw new DataBaseException("Ya existe un director con ese nombre");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool IsNameUnique(string name)
        {
            var validationInt = directors.FindIndex(dir => dir.Name.Equals(name));
            return (validationInt == -1);
        }

        public void DeleteDirector(string directorName)
        {
            queue.ChckAndAddToDirectorList(directorName);

            try
            {
                semaphore.WaitAsync();
                var indexToDelete = directors.FindIndex(dir => dir.Name.Equals(directorName));
                if (indexToDelete == -1)
                {
                    queue.RemoveDirectorFromQueue(directorName);
                    throw new DataBaseException("No se encontro el director solicitado");
                }
                directors.RemoveAt(indexToDelete);
            }
            finally
            {
                semaphore.Release();
                queue.RemoveDirectorFromQueue(directorName);
            }
        }

        public List<Director> GetDirectors()
        {
            try
            {
                semaphore.WaitAsync();
                return directors;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void UpdateDirector(string currentName, Director updatedDirector)
        {
            queue.ChckAndAddToDirectorList(currentName);
            try
            {
                semaphore.WaitAsync();
                var indexToModify = directors.FindIndex(dir => dir.Name.Equals(currentName));
                if (indexToModify == -1)
                {
                    queue.RemoveDirectorFromQueue(currentName);
                    throw new DataBaseException("No se encontro el director solicitado");
                }
                directors[indexToModify] = updatedDirector;
            }
            finally
            {
                semaphore.Release();
                queue.RemoveDirectorFromQueue(currentName);
            }
        }

        public Director GetDirector(string name)
        {
            try
            {
                semaphore.WaitAsync();
                var indexToReturn = directors.FindIndex(dir => dir.Name.Equals(name));
                if (indexToReturn == -1)
                {
                    throw new DataBaseException("No se encontro el director solicitado");
                }
                return directors[indexToReturn];
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}