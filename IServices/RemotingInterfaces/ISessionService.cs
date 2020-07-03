using System;

namespace IServices
{
    public interface ISessionService
    {
        bool IsValidToken(string token);

        Guid? CreateToken(string userName, string password);

        bool DeleteLoggedUser(string token);

        string GetUserByToken(string token);
    }
}
