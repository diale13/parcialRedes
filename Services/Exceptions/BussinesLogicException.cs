using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BussinesLogicException : Exception
    {
        public BussinesLogicException(string msg) : base(msg)
        {

        }
    }      
}
