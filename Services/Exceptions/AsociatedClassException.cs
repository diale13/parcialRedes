using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AsociatedClassException : Exception
    {
        public AsociatedClassException(string msg) : base(msg)
        {

        }
    }
}
