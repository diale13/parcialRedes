using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFileHandler
    {
        byte[] ReadData(string path, long offset, int length);
        void Write(string fileName, byte[] data);
        void WriteFrame(byte[] data);
        byte[] ReadFrame(int length);
    }
}
