using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileFunctions : IFileFunctions
    {


        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            if (!FileExists(path))
            {
                throw new FileNotFoundException("The specified file does not exist, please verify the given path");
            }

            return new FileInfo(path).Name;
        }

        public long GetFileSize(string path)
        {
            if (!FileExists(path))
            {
                throw new FileNotFoundException("The specified file does not exist, please verify the given path");
            }

            return new FileInfo(path).Length;
        }
    }
}
