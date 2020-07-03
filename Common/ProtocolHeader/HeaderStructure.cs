using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class HeaderStructure
    {
        public Command Command { get; set; }
        public Flag Flag { get; set; }
        public HeaderLength HeaderLength { get; set; }

        public FlagType FlagType { get; set; }
        public CommandType CommandType { get; set; }

        public int ActualLength { get; set; }

        public HeaderStructure()
        {

        }

        public HeaderStructure(FlagType flagType, CommandType commandType, int actualLength)
        {
            this.Command = new Command();
            this.Flag = new Flag();
            this.HeaderLength = new HeaderLength();

            this.FlagType = flagType;
            this.CommandType = commandType;
            this.ActualLength = actualLength;
        }


    }
}
