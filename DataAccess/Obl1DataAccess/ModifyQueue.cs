using DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DataAccess
{
    public class ModifyQueue
    {

        private static ModifyQueue queueInstance;

        private List<string> genreNamesQueue;
        private List<string> movieNamesQueue;
        private List<string> direcorNamesQueue;


        private static SemaphoreSlim movieSemaphore = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim genreSemaphore = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim directorSemaphore = new SemaphoreSlim(1, 1);


        private ModifyQueue()
        {
            this.genreNamesQueue = new List<string>();
            this.movieNamesQueue = new List<string>();
            this.direcorNamesQueue = new List<string>();
        }


        public void ChckAndAddToMovieList(string movieToCheck)
        {
            try
            {
                movieSemaphore.WaitAsync();
                int indexToCheck = movieNamesQueue.FindIndex(movieNameInList => movieNameInList.Equals(movieToCheck));
                if (indexToCheck == -1)
                {
                    movieNamesQueue.Add(movieToCheck);
                }
                else
                {
                    throw new EntityBeingModifiedException("Se ingereso una pelicula que esta siendo modificada, espere unos minutos y vuelva a intentar");
                }
            }
            finally
            {
                movieSemaphore.Release();
            }
        }

        public void RemoveMovieFromModifyQueue(string movieThatWasModified)
        {
            try
            {
                movieSemaphore.WaitAsync();
                this.movieNamesQueue.Remove(movieThatWasModified);
            }
            finally
            {
                movieSemaphore.Release();
            }
        }

        public void ChckAndAddToGenreList(string genreToCheck)
        {
            try
            {
                genreSemaphore.WaitAsync();

                int indexToCheck = genreNamesQueue.FindIndex(genreNameInList => genreNameInList.Equals(genreToCheck));
                if (indexToCheck == -1)
                {
                    genreNamesQueue.Add(genreToCheck);
                }
                else
                {
                    throw new EntityBeingModifiedException("Se ingereso un genero que esta siendo modificado, espere unos minutos y vuelva a intentar");
                }
            }
            finally
            {
                genreSemaphore.Release();
            }
        }

        public void RemoveGenreFromQueue(string genreModified)
        {

            try
            {
                genreSemaphore.WaitAsync();
                this.genreNamesQueue.Remove(genreModified);
            }
            finally
            {
                genreSemaphore.Release();
            }
        }

        public void ChckAndAddToDirectorList(string directorToCheck)
        {
            try
            {
                directorSemaphore.WaitAsync();
                int indexToCheck = direcorNamesQueue.FindIndex(dirNameInList => dirNameInList.Equals(directorToCheck));
                if (indexToCheck == -1)
                {
                    direcorNamesQueue.Add(directorToCheck);
                }
                else
                {
                    throw new EntityBeingModifiedException("Se ingereso un director que esta siendo modificado, espere unos minutos y vuelva a intentar");
                }
            }
            finally
            {
                directorSemaphore.Release();
            }
        }

        public void RemoveDirectorFromQueue(string dirModified)
        {

            try
            {
                directorSemaphore.WaitAsync();
                this.direcorNamesQueue.Remove(dirModified);
            }
            finally
            {
                directorSemaphore.Release();
            }
        }


        public static ModifyQueue GetInstance()
        {
            if (queueInstance == null)
            {
                queueInstance = new ModifyQueue();
                return queueInstance;
            }
            else
            {
                return queueInstance;
            }
        }





    }
}
