using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IDirectorService
    {
        List<Director> GetDirectors();
        void AddDirector(Director dir);
        void DeleteDirector(Director name);
        void UpdateDirector(string oldDirName, Director updatedDir);
        Director GetDirector(string dirName);

    }
}