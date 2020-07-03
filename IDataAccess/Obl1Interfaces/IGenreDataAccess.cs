using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IGenreDataAccess
    {
        void Upload(Genre genre);
        void Update(string genreName, Genre updatedGenre);
        void Delete(Genre genreToDelete);
        Genre GetGenre(string name);
        bool Exists(string genre);
        List<Genre> GetGenres();
    }
}

