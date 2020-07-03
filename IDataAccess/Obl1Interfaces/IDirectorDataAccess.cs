using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IDirectorDataAccess
    {
        List<Director> GetDirectors();

        void AddDirector(Director dir);
        void DeleteDirector(string name);
        void UpdateDirector(string currentName, Director updatedDirector);
        Director GetDirector(string name);
    }
}