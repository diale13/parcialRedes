using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAsociationService
    {
        void DeAsociateGenreMovie(Movie movie, Genre genre);
        void AsociateGenreToMovie(Movie movie, Genre genre);
        void AsociateDirectorMovie(Movie movie, Director director);
        void DeAsociatDirMovie(Movie movie, Director director);
    }
}
