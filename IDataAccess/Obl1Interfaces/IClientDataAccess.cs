using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IClientDataAccess
    {
        List<User> GetClients();
        int AmountOfClients();
        int AddClient(string dateTime);
        void RemoveClient(int userId);
    }
}
