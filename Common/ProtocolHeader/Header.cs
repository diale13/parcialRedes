using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Header : ICodification<HeaderStructure>
    {
        public static int HEADER_LENGTH = 9;

        private FlagType flagType;
        private CommandType commandType;
        private int length;

        Command command;
        Flag flag;
        HeaderLength headerLength;

        public Header()
        {
            this.command = new Command();
            this.flag = new Flag();
            this.headerLength = new HeaderLength();
        }


        public Header(HeaderStructure headerStructure)
        {
            this.command = headerStructure.Command;
            this.flag = headerStructure.Flag;
            this.headerLength = headerStructure.HeaderLength;
        }




        public HeaderStructure Decode(byte[] message)
        {
            int read = 0;
            Byte[] flagByteInfo = ReadFrame(message, read, Flag.ASSIGNED_BYTES);
            read += Flag.ASSIGNED_BYTES;
            Byte[] commandByteInfo = ReadFrame(message, read, Command.ASSIGNED_BYTES);
            read += Command.ASSIGNED_BYTES;
            Byte[] lengthBytesInfo = ReadFrame(message, read, HeaderLength.ASSIGNED_BYTES);
            //read += HeaderLength.ASSIGNED_BYTES;
            
            FlagType headerType = flag.Decode(flagByteInfo);
            CommandType cmdType = command.Decode(commandByteInfo);
            int length = headerLength.Decode(lengthBytesInfo);

            return new HeaderStructure(headerType, cmdType, length);
        }

        private Byte[] ReadFrame(Byte[] message, int skip, int take)
        {
            return message
                .Skip(skip)
                .Take(take)
                .ToArray();
        }

        public byte[] Encode(HeaderStructure header)
        {
            Byte[] encodedFlag = header.Flag.Encode(flagType);
            Byte[] encodedCommand = header.Command.Encode(commandType);
            Byte[] encodedLength = header.HeaderLength.Encode(length);

            Byte[] resultEncoded = encodedFlag
                .Concat(encodedCommand)
                .Concat(encodedLength)
                .ToArray();
            return resultEncoded;
        }
    }
}
