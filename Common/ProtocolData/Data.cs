using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Data : ICodification<String>
    {
        public Byte[] Encode(String data)
        {
            return Encoding.ASCII.GetBytes(data);
        }
        public String Decode(Byte[] message)
        {
            String decoded = Encoding.ASCII.GetString(message);
            return decoded;
        }
    }
}
