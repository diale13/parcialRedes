using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Command : ICodification<CommandType>
    {
        public static int ASSIGNED_BYTES = 2;
        public CommandType Decode(byte[] message)
        {
            String decoded = Encoding.ASCII.GetString(message);
            CommandType parsed = (CommandType)Enum.Parse(typeof(CommandType), decoded);
            return parsed;
        }

        public byte[] Encode(CommandType type)
        {
            Byte[] command = Encoding.ASCII.GetBytes(type.ToString());
            Array.Resize(ref command, ASSIGNED_BYTES);
            return command;
        }
    }
}
