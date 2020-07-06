using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Identifier { get; set; }
        public string Time { get; set; }

        public override string ToString()
        {
            string ret = "Id: " + Identifier + " hora de conexion: " + Time;
            return ret;
        }


    }
}
