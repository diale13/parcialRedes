using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFrameHandler
    {
        int WordLength { get; }
        Task<byte[]> ReadDataAsync();
        Task SendMessageAsync(byte[] data);
    }
}
