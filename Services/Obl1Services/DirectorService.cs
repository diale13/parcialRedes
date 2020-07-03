using Domain;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDataAccess;

namespace Services
{
    public class DirectorService : IDirectorService
    {
        public IDirectorDataAccess directorDataAccess;

        public DirectorService(IDirectorDataAccess iDirectorDataAccess)
        {
            this.directorDataAccess = iDirectorDataAccess;
        }

        public void AddDirector(Director dir)
        {
            if (!IsDirectorValid(dir))
            {
                throw new BussinesLogicException("Un director debe tener un nombre");
            }
            directorDataAccess.AddDirector(dir);
        }

        private bool IsDirectorValid(Director dir)
        {
            return dir.Name != "";
        }

        public void DeleteDirector(Director dir)
        {
            if(dir.DirectedMovies.Count() == 0)
            {
                throw new BussinesLogicException("Un director no puede ser borrado si esta asociado a una pelicula");
            }

            directorDataAccess.DeleteDirector(dir.Name);
        }

        public List<Director> GetDirectors()
        {
            List<Director> directors = directorDataAccess.GetDirectors();
            return directors;
        }

        public void UpdateDirector(string oldName, Director updatedDirector)
        {
            directorDataAccess.UpdateDirector(oldName, updatedDirector);
        }

        public Director GetDirector(string dirName)
        {
           return directorDataAccess.GetDirector(dirName);
        }
    }
}