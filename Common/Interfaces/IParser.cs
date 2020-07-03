using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IParser
    {
        String[] GetDataObject(byte[] stream);
        List<string> GetList(string v);
    }
}
