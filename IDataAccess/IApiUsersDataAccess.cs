using Domain;
using System.Collections.Generic;

namespace IDataAccess
{
    public interface IApiUsersDataAccess
    {
        List<ApiUser> GetAll();
        ApiUser Get(string nickname);
        void Add(ApiUser userToAdd);
        void Update(ApiUser userToUpdate);
        void Delete(string nickname, string password);
    }
}
