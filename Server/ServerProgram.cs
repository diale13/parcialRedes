using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using DataAccess;
using DataAccess.Exceptions;
using IDataAccess;
using IServices;
using Services;

namespace Server
{
    public class ServerProgram
    {
        static readonly ISettingsManager SettingsMgr = new SettingsManager();
        static bool isServerUp = true;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Server is starting...");
            var ipEndPoint = new IPEndPoint(
                IPAddress.Parse(SettingsMgr.ReadSetting(ServerConfig.ServerIpConfigKey)),
                int.Parse(SettingsMgr.ReadSetting(ServerConfig.SeverPortConfigKey)));
            var tcpListener = new TcpListener(ipEndPoint);

            tcpListener.Start(100);
            Console.WriteLine("Server started listening connections on ip " + SettingsMgr.ReadSetting(ServerConfig.ServerIpConfigKey) + " and port " + SettingsMgr.ReadSetting(ServerConfig.SeverPortConfigKey));
            Console.WriteLine("Server will start displaying messages from the clients");

            var sessionServiceChannel = RemotingManager.InitiateRemotingSessionService();
            var apiUserServiceChannel = RemotingManager.InitiateRemotingApiUserService();
            var movieServiceChannel = RemotingManager.InitiateRemotingMovieService();
            var logServiceChannel = RemotingManager.InitiateLogService();

            try
            {
                Task serverFunctionsTask = Task.Run(() => ServerFunctions());
                while (isServerUp)
                {
                    var tcpClientSocket = await tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                    var handleClientTask = Task.Run(async () => await HandleClient(tcpClientSocket).ConfigureAwait(false));
                }
            }
            finally
            {
                ChannelServices.UnregisterChannel(sessionServiceChannel);
                ChannelServices.UnregisterChannel(apiUserServiceChannel);
                ChannelServices.UnregisterChannel(movieServiceChannel);
                ChannelServices.UnregisterChannel(logServiceChannel);
            }
        }

        static void ServerFunctions()
        {
            while (isServerUp)
            {
                var serverFunction = Console.ReadLine();
                Console.WriteLine("Server running: " + serverFunction);
                HandleServer(serverFunction);
            }
        }
        private static bool HandleServer(string serverFunction)
        {
            switch (serverFunction)
            {
                case "cls":
                    Console.Clear();
                    break;
                case "clientList":
                    ServerFunctionManager.DisplayClients();
                    break;
                case "genreList":
                    ServerFunctionManager.DisplayGenres();
                    break;
                case "movieList_added":
                    ServerFunctionManager.DisplayMoviesAddedOrder();
                    break;
                case "movieList_date":
                    ServerFunctionManager.DisplayMoviesByDate();
                    break;
                case "movieList_director":
                    ServerFunctionManager.DisplayMoviesByDirector();
                    break;
                case "movieList_genre":
                    ServerFunctionManager.DisplayMoviesByGenre();
                    break;
                case "movie_files":
                    string moviename = Console.ReadLine();
                    ServerFunctionManager.DisplayMovieFiles(moviename);
                    break;
                case "SHUTDOWN":
                    isServerUp = false;
                    Console.WriteLine("Server won`t be accepting new clients until turned on");
                    break;
                case "STARTUP":
                    isServerUp = true;
                    Console.WriteLine("Server online");
                    break;
                default:
                    Console.WriteLine(serverFunction + " no es un comando del servidor");
                    PrintServerCommands();
                    break;
            }
            return true;
        }
        private static void PrintServerCommands()
        {
            Console.WriteLine("----SERVER COMMANDS----");
            Console.WriteLine("clientList");
            Console.WriteLine("genreList");
            Console.WriteLine("movieList_added");
            Console.WriteLine("movieList_date");
            Console.WriteLine("movieList_director");
            Console.WriteLine("movieList_genre");
            Console.WriteLine("movie_files");
            Console.WriteLine("SHUTDOWN");
            Console.WriteLine("STARTUP");
            Console.WriteLine("cls");
            Console.WriteLine("----");
        }

        private static async Task HandleClient(TcpClient tcpClientSocket)
        {
            var isClientConnected = true;

            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);

            DateTime hour = DateTime.Now;
            string connectionHour = hour.Hour + ":" + hour.Minute;

            int token = clientService.AddClient(connectionHour);

            try
            {
                using (var networkStream = tcpClientSocket.GetStream())
                {
                    SendMenu(networkStream);

                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    while (isClientConnected)
                    {
                        var frame = await frameHandler.ReadDataAsync();
                        var printiame = Encoding.ASCII.GetString(frame);
                        Console.WriteLine("Now handeling: " + printiame);
                        isClientConnected = HandleRequest(frame, networkStream);
                    }
                    clientService.RemoveClient(token);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("The client connection was interrupted");
            }
        }

        private static void ServerResponse(IFrameHandler frameHandler, string msg)
        {
            var bytesMsg = Encoding.UTF8.GetBytes(msg);
            frameHandler.SendMessageAsync(bytesMsg);
        }

        private static bool HandleRequest(byte[] frame, NetworkStream networkStream)
        {
            try
            {
                IFrameHandler frameHandler = new FrameHandler(networkStream);

                ICodification<HeaderStructure> header = new Header();
               
                var logManager = new SenderService();
                int logSize = 0;

                HeaderStructure headerStructure = header.Decode(frame);

                if (!IsHeaderStructureOk(headerStructure, frame))
                {
                    return true;
                }

                var command = headerStructure.CommandType;
                
                switch (command)
                {
                    case CommandType.AG:
                        ServerGenreManager.UploadGenre(frame);
                        ServerResponse(frameHandler, "Genre Uploaded");
                        logSize = 2;
                        break;

                    case CommandType.BG:
                        ServerGenreManager.DeleteGenre(frame);
                        ServerResponse(frameHandler, "Genre Deleted");
                        logSize = 1;
                        break;

                    case CommandType.MG:
                        ServerGenreManager.ModifyGenre(frame);
                        ServerResponse(frameHandler, "Genre modified");
                        logSize = 3;
                        break;

                    case CommandType.AP:
                        ServerMovieManager.Upload(frame);
                        ServerResponse(frameHandler, "Movie uploaded");
                        logSize = 3;
                        break;

                    case CommandType.BP:
                        ServerMovieManager.Delete(frame);
                        ServerResponse(frameHandler, "Movie deleted");
                        logSize = 1;
                        break;

                    case CommandType.MP:
                        ServerMovieManager.Modify(frame);
                        ServerResponse(frameHandler, "Movie updated");
                        logSize = 4;
                        break;

                    case CommandType.AS:
                        ServerAsociationManager.AsociateGenreToMovie(frame);
                        ServerResponse(frameHandler, "Movie and genre associated");
                        logSize = 2;
                        break;

                    case CommandType.DS:
                        ServerAsociationManager.DeAsociateGenreToMovie(frame);
                        ServerResponse(frameHandler, "Movie and genre deassociated");
                        logSize = 2;
                        break;

                    case CommandType.AD:
                        ServerDirectorManager.UploadDirector(frame);
                        ServerResponse(frameHandler, "Director uploaded");
                        logSize = 4;
                        break;

                    case CommandType.BD:
                        ServerDirectorManager.DeleteDirector(frame);
                        ServerResponse(frameHandler, "Director deleted");
                        logSize = 1;
                        break;

                    case CommandType.MD:
                        ServerDirectorManager.ModifyDirector(frame);
                        ServerResponse(frameHandler, "Director updated");
                        logSize = 5;
                        break;

                    case CommandType.SA:
                        string fileName = ServerMovieManager.SaveFile(frame, networkStream);
                        ServerResponse(frameHandler, "Sending file...@" + fileName);
                        ServerMovieManager.ReceiveFile(networkStream);
                        logSize = 2;
                        ServerResponse(frameHandler, "File sent");
                        break;

                    case CommandType.DM:
                        ServerAsociationManager.AsociateDirectorToMovie(frame);
                        ServerResponse(frameHandler, "Director and movie asociated");
                        logSize = 2;
                        break;

                    case CommandType.DD:
                        ServerAsociationManager.DeAsociateDirectorToMovie(frame);
                        ServerResponse(frameHandler, "Director and movie deasociated");
                        logSize = 2;
                        break;

                    case CommandType.FF:
                        ServerResponse(frameHandler, "Goodbye");
                        logSize = 1;
                        return false;

                    default:
                        ServerResponse(frameHandler, "Formato de trama invalido vuelva a enviar");
                        break;
                }                
                logManager.CreateLog(command.ToString().ToUpper(), frame, logSize);
                return true;
            }
            catch (Exception ex)
            {
                var logManager = new SenderService();
                logManager.CreateLog("EX", frame, 1);
                if (ex is FormatException || ex is ArgumentException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, "La trama recibida fue invalida");
                }
                else if (ex is IndexOutOfRangeException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, "El objeto solicitado en el Data esta mal construido");
                }
                else if (ex is BussinesLogicException || ex is AsociatedClassException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, ex.Message);
                }
                else if (ex is DataBaseException || ex is EntityBeingModifiedException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, ex.Message);
                }
                else
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, "Ha ocurrido un error inesperado vuelva a mandar su trama");
                }
                return true;
            }
        }

        private static bool IsHeaderStructureOk(HeaderStructure headerStructure, byte[] frame)
        {
            return headerStructure.CommandType.GetType().IsEnum && headerStructure.FlagType.GetType().IsEnum;
        }

        static void SendMenu(NetworkStream networkStream)
        {
            IMenuDataAccess menuDA = new MenuDataAccess();
            IMenuService menuService = new MenuService(menuDA);
            IFrameHandler frameHandler = new FrameHandler(networkStream);
            var items = menuService.GetMenuItems();


            string menu = "";
            int count = 1;
            foreach (var item in items)
            {
                menu += count + ") " + item + "\n";
                count++;
            }

            byte[] data = Encoding.UTF8.GetBytes(menu);

            frameHandler.SendMessageAsync(data);
        }


    }
}
