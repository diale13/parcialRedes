using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common
{
    public class FrameHandler : IFrameHandler
    {
        public int WordLength { get; }
        private readonly NetworkStream networkStream;

        public FrameHandler(NetworkStream aNetworkStream)
        {
            WordLength = 4; 
            networkStream = aNetworkStream;
        }
             
        public async Task SendMessageAsync(byte[] mensaje)
        {
            byte[] dataLength = BitConverter.GetBytes(mensaje.Length);
            await networkStream.WriteAsync(dataLength, 0, this.WordLength);
            await networkStream.WriteAsync(mensaje, 0, mensaje.Length);
        }

        public async Task<byte[]> ReadDataAsync()
        {
            var dataLength = new byte[WordLength];
            int totalReceived = 0;
            while (totalReceived < WordLength)
            {
                int received = await networkStream.ReadAsync(dataLength, totalReceived, WordLength - totalReceived).ConfigureAwait(false);
                if (received == 0) 
                {
                    throw new SocketException();
                }
                totalReceived += received;
            }
            var actualLength = BitConverter.ToInt32(dataLength, 0);
            var data = new byte[actualLength];
            totalReceived = 0;
            while (totalReceived < actualLength)
            {
                int received = await networkStream.ReadAsync(data, totalReceived, actualLength - totalReceived).ConfigureAwait(false);
                if (received == 0)
                {
                    throw new SocketException();
                }
                totalReceived += received;
            }
            return data;
        }
    }
}
