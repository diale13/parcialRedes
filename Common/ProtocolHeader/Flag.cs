using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Flag : ICodification<FlagType>
    {
        public static int ASSIGNED_BYTES = 3;

        public Byte[] Encode(FlagType type)
        {
            Byte[] encodedFlag = Encoding.ASCII.GetBytes(type.ToString());
            Array.Resize(ref encodedFlag, ASSIGNED_BYTES);
            return encodedFlag;
        }

        public FlagType Decode(Byte[] message)
        {
            String decoded = Encoding.ASCII.GetString(message);
            FlagType parsed = (FlagType)Enum.Parse(typeof(FlagType), decoded);
            return parsed;
        }
    }
}
