
using Common.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Parser : IParser
    {
        public string[] GetDataObject(byte[] fullFrame)
        {
            var data = fullFrame.Skip(Header.HEADER_LENGTH).ToArray();
            string stream = Encoding.ASCII.GetString(data); 
            string[] separator = { "@" };

            return stream.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public List<string> GetList(string v)
        {
            string[] separator = { "#" };
            var listToBeMade = v.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var listMade = new List<string>();
            foreach (var item in listToBeMade)
            {
                listMade.Add(item);
            }
            return listMade;
        }
    }
}
