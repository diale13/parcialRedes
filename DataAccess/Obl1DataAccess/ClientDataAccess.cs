using System.Collections.Generic;
using System.Threading;
using Domain;
using IDataAccess;
using Persistance;

namespace DataAccess
{
    public class ClientDataAccess : IClientDataAccess
    {
        private List<User> users = MemoryDataBase.GetInstance().Users;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public List<User> GetClients()
        {
            try
            {
                semaphore.WaitAsync();
                return users;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public int AmountOfClients()
        {
            try
            {
                semaphore.WaitAsync();
                return users.Count;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public int AddClient(string dateTime)
        {
            try
            {
                semaphore.WaitAsync();
                int token = MemoryDataBase.GetInstance().UserTokenCount + 1;
                MemoryDataBase.GetInstance().UserTokenCount = MemoryDataBase.GetInstance().UserTokenCount + 1;
                User userToBeAdded = new User
                {
                    Id = token,
                    Time = dateTime
                };
                users.Add(userToBeAdded);
                return token;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void RemoveClient(int userId)
        {
            try
            {
                semaphore.WaitAsync();
                var indexToDelete = users.FindIndex(userInList => userInList.Id.Equals(userInList.Id));
                users.RemoveAt(indexToDelete);
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
}
