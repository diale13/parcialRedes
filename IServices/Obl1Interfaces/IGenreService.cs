using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IGenreService
    {
        void Upload(Genre genToUpload);
        void Delete(Genre genre);
        void Update(string genreName, Genre updatedGenre);
        Genre GetGenre(string name);
        bool Exists(string genre);
        List<Genre> GetGenres();
    }
}
