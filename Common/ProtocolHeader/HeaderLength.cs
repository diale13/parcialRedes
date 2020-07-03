using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class HeaderLength : ICodification<int>
    {
        public static int ASSIGNED_BYTES = 4;

        public Byte[] Encode(int length)
        {
            Byte[] encodedDataLength = Encoding.ASCII.GetBytes(length.ToString());
            Array.Resize(ref encodedDataLength, ASSIGNED_BYTES);
            return encodedDataLength;
        }

        public int Decode(Byte[] message)
        {
            String decoded = Encoding.ASCII.GetString(message);
            return int.Parse(decoded);
        }
    }
}
