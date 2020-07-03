using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IClientService
    {
        int AddClient(string connectionTime);
        void RemoveClient(int clientToken);
        List<User> GetConnectedClients();
    }
}
