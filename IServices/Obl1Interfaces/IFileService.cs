using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IFileService
    {
        void SendFile(string path);
        int GetLength();
        void ReceiveFile();
    }
}
