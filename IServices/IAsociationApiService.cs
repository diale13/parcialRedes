namespace IServices
{
    public interface IAsociationApiService
    {
        void DeAsociateGenreMovie(string movieName, string genreName);
        void AsociateGenreToMovie(string movieName, string genreName);
        void AsociateDirectorMovie(string movieName, string directorName);
        void DeAsociatDirMovie(string movieName, string directorName);
    }
}
