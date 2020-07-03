using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public class FileHandler : IFileHandler
    {
        private readonly NetworkStream networkStream;

        public FileHandler(NetworkStream aNetworkStream)
        {
            networkStream = aNetworkStream;
        }

        public byte[] ReadData(string path, long offset, int length)
        {
            var data = new byte[length];

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                fileStream.Position = offset;
                var bytesRead = 0;
                while (bytesRead < length)
                {
                    var read = fileStream.Read(data, bytesRead, length - bytesRead);
                    if (read == 0)
                    {
                        throw new Exception("No se pudo leer el archivo"); //TODO: arreglar excepcion
                    }
                    bytesRead += read;
                }
            }
            return data;
        }

        public void WriteFrame(byte[] data)
        {
            networkStream.Write(data, 0, data.Length);
        }

        public void Write(string fileName, byte[] data)
        {
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Append))
                {
                    fileStream.Write(data, 0, data.Length);
                }
            }
            else
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    fileStream.Write(data, 0, data.Length);
                }
            }
        }

        public byte[] ReadFrame(int length)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                var received = networkStream.Read(data, dataReceived, length - dataReceived);
                if (received == 0)
                {
                    throw new SocketException();
                }
                dataReceived += received;
            }

            return data;
        }
    }
}
