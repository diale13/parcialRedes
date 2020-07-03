using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using IServices;
using Services;

namespace Client
{
    class ClientProgram
    {

        static async Task Main(string[] args)
        {
            try
            {
                var tcpClient = await ClientConnectAsync().ConfigureAwait(false);
                var keepConnection = true;
                using (var networkStream = tcpClient.GetStream())
                {
                    FrameHandler frameHandler = new FrameHandler(networkStream);

                    var serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                    var menu = Encoding.ASCII.GetString(serverResponse);
                    Console.WriteLine(menu);

                    while (keepConnection)
                    {
                        var frameToBeSent = "";

                        while (frameToBeSent == "")
                        {
                            frameToBeSent = Console.ReadLine();
                        }                      

                        if (frameToBeSent.Equals("exit"))
                        {
                            keepConnection = false;
                            var exitFrame = Encoding.UTF8.GetBytes("REQFF0000");
                            var tarea = frameHandler.SendMessageAsync(exitFrame);
                            serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                            var asciiResponse = (Encoding.ASCII.GetString(serverResponse));
                            Console.WriteLine(asciiResponse);
                        }
                        else
                        {
                            var encodedFrame = Encoding.UTF8.GetBytes(frameToBeSent);
                            var tarea = frameHandler.SendMessageAsync(encodedFrame);
                            serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                            var asciiResponse = (Encoding.ASCII.GetString(serverResponse));
                            string[] separator = { "@" };
                            string[] splittedResponse = asciiResponse.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(splittedResponse[0]);
                            if (splittedResponse[0].Equals("Sending file..."))
                            {
                                IFileFunctions fileFunctions = new FileFunctions();
                                IFileHandler fileHandler = new FileHandler(networkStream);
                                IFileService fileService = new FileService(fileFunctions, fileHandler);
                                try
                                {
                                    fileService.SendFile(splittedResponse[1]);
                                    serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                                    asciiResponse = (Encoding.ASCII.GetString(serverResponse));
                                    Console.WriteLine(asciiResponse);
                                }
                                catch (BussinesLogicException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            if (asciiResponse.Equals("Goodbye"))
                            {
                                keepConnection = false;
                            }
                        }


                    }
                }
                tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ha ocurrido un error de conexion verifique su app config o que el servidor este levantado");
                Console.ReadLine();
            }
        }

        private static async Task<TcpClient> ClientConnectAsync()
        {
            Console.WriteLine("Client starting...");
            var applicationSettings = ConfigurationManager.GetSection("appSettings") as NameValueCollection;
            string clientIpAddress = applicationSettings.Get("ClientIpAddress");
            int clientPort = Int32.Parse(applicationSettings.Get("ClientPort"));
            string serverIpAddress = applicationSettings.Get("ServerIpAddress");
            int serverPort = Int32.Parse(applicationSettings.Get("ServerPort"));
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse(clientIpAddress), clientPort);
            var tcpClient = new TcpClient(clientIpEndPoint);
            Console.WriteLine("Trying to connect to server");

            await tcpClient.ConnectAsync(serverIpAddress, serverPort).ConfigureAwait(false);
            return tcpClient;
        }



    }
}
