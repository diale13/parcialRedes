using Common;
using Common.Interfaces;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileService : IFileService
    {
        public IFileFunctions fileHelper;
        public IFileHandler fileHandler;

        public FileService(IFileFunctions iFileHelper, IFileHandler iFileHandler)
        {
            fileHelper = iFileHelper;
            fileHandler = iFileHandler;
        }

        public int GetLength()
        {
            return Specification.FixedFileNameLength + Specification.FixedFileSizeLength;
        }

        public void ReceiveFile()
        {
            var stream = fileHandler.ReadFrame(GetLength());
            var fileNameSize = BitConverter.ToInt32(stream, 0);
            var fileSize = BitConverter.ToInt64(stream, Specification.FixedFileNameLength);

            var fileName = Encoding.UTF8.GetString(fileHandler.ReadFrame(fileNameSize));

            long parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;
            
            while(fileSize > offset)
            {
                byte[] data;
                if(currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = fileHandler.ReadFrame(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = fileHandler.ReadFrame(Specification.MaxPacketSize);
                    offset += Specification.MaxPacketSize;
                    currentPart++;
                }
                fileHandler.Write(fileName, data);
            }
        }

        public void SendFile(string path)
        {
            long fileSize = fileHelper.GetFileSize(path);
            string fileName = fileHelper.GetFileName(path);
            if (fileSize > 104857600)
                throw new BussinesLogicException("The file is too big");
            var stream = Create(fileName, fileSize);

            fileHandler.WriteFrame(stream);

            fileHandler.WriteFrame(Encoding.UTF8.GetBytes(fileName));

            long parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while(fileSize > offset)
            {
                byte[] data;
                if(currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = fileHandler.ReadData(path, offset, lastPartSize);
                    offset += lastPartSize;
                } else
                {
                    data = fileHandler.ReadData(path, offset, Specification.MaxPacketSize);
                    offset += Specification.MaxPacketSize;
                }

                fileHandler.WriteFrame(data);

                currentPart++;
            }
        }

        public Byte[] Create(string fileName, long fileSize)
        {
            var stream = new byte[GetLength()];
            var fileNameData = BitConverter.GetBytes(Encoding.UTF8.GetBytes(fileName).Length);
            if (fileNameData.Length != Specification.FixedFileNameLength)
                throw new Exception("Hay un problema con el nombre del archivo");
            var fileSizeData = BitConverter.GetBytes(fileSize);

            Array.Copy(fileNameData, 0, stream, 0, Specification.FixedFileNameLength);
            Array.Copy(fileSizeData, 0, stream, Specification.FixedFileNameLength, Specification.FixedFileSizeLength);

            return stream;
        }
    }
}
