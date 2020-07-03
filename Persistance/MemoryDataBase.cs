using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class MemoryDataBase
    {
        private static MemoryDataBase singletonMemoryDB;

        public List<Genre> Genres { get; set; }
        public List<Movie> Movies { get; set; }
        public List<Director> Directors { get; set; }
        public List<User> Users { get; set; }
        public int UserTokenCount { get; set; }
        public List<ApiUser> ApiUsers { get; set; }


        private MemoryDataBase()
        {
            this.ApiUsers = new List<ApiUser>();
            this.Genres = new List<Genre>();
            this.Movies = new List<Movie>();
            this.Directors = new List<Director>();
            this.Users = new List<User>();
            UserTokenCount = -1;
        }

        public static MemoryDataBase GetInstance()
        {
            if (singletonMemoryDB == null)
            {
                singletonMemoryDB = new MemoryDataBase();
                return singletonMemoryDB;
            }
            else
            {
                return singletonMemoryDB;
            }
        }

    }
}
