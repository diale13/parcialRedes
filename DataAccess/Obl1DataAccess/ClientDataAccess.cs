using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;
using Persistance;

namespace DataAccess
{
    public class ClientDataAccess : IClientDataAccess
    {
        //private List<User> users = MemoryDataBase.GetInstance().Users;
        //private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private Context context;

        public ClientDataAccess()
        {
            context = new Context();
        }

        public List<User> GetClients()
        {
            try
            {
                //semaphore.WaitAsync();
                //return users;
                context.Database.Exists();
                return context.Users.ToList();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException("No se pudieron obtener los usuarios correctamente");
            }
        }

        public int AmountOfClients()
        {
            try
            {
                context.Database.Exists();
                return context.Users.Count();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new DataBaseException("No se pudo obtener la cantidad de clientes");
            }
        }

        public int AddClient(string dateTime)
        {
            int token = AmountOfClients() + 1;
            try
            {
                User userToBeAdded = new User
                {
                    Identifier = token,
                    Time = dateTime
                };
                context.Set<User>().Add(userToBeAdded);
                context.SaveChanges();
                return token;
            //    semaphore.WaitAsync();
            //    int token = MemoryDataBase.GetInstance().UserTokenCount + 1;
            //    MemoryDataBase.GetInstance().UserTokenCount = MemoryDataBase.GetInstance().UserTokenCount + 1;
            //    User userToBeAdded = new User
            //    {
            //        Identifier = token,
            //        Time = dateTime
            //    };
            //    users.Add(userToBeAdded);
            //    return token;
            //}
            //finally
            //{
            //    semaphore.Release();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo cargar el cliente {token} en la base de datos");
            }
        }

        public void RemoveClient(int userId)
        {
            try
            {
                //semaphore.WaitAsync();
                //var indexToDelete = users.FindIndex(userInList => userInList.Identifier.Equals(userInList.Identifier));
                //users.RemoveAt(indexToDelete);
                User userToRemove = GetUser(userId);
                context.Users.Remove(userToRemove);
                context.SaveChangesAsync();
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo eliminar el usuario {userId}");
            }
        }

        private User GetUser(int userId)
        {
            try
            {
                User user = context.Users.Find(userId);
                return user;
            }
            catch (DbException)
            {
                throw new DataBaseException("No se pudo conectar con la base de datos");
            }
            catch (Exception)
            {
                throw new DataBaseException($"No se pudo encontrar al usuario con el id {userId}");
            }
        }
    }
}
