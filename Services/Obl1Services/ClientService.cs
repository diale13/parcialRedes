using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using IDataAccess;
using IServices;

namespace Services
{
    public class ClientService : IClientService
    {
        public IClientDataAccess clientsDataAccess;
        public ClientService(IClientDataAccess iClientsDataAccess)
        {
            this.clientsDataAccess = iClientsDataAccess;
        }

        public int AddClient(string connectionTime)
        {
            var addedClientToken = clientsDataAccess.AddClient(connectionTime);
            return addedClientToken;
        }

      
        public List<User> GetConnectedClients()
        {
            List<User> clients = clientsDataAccess.GetClients();
            return clients;
        }

        public void RemoveClient(int clientToken)
        {
            clientsDataAccess.RemoveClient(clientToken);
        }
    }
}
