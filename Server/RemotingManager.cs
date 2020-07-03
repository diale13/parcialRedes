using Common;
using Common.Interfaces;
using Services;
using Services.RemotingServices;
using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace Server
{
    public static class RemotingManager
    {
        static readonly ISettingsManager SettingsMgr = new SettingsManager();
        public static TcpChannel InitiateRemotingSessionService()
        {
            var port = Int32.Parse(SettingsMgr.ReadSetting(ServerConfig.SessionServicePort));
            var tcpSessionChannel = (TcpChannel)GetChannel("sessionChannel", port, false);
            ChannelServices.RegisterChannel(tcpSessionChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(SessionService),
                "SessionService",
                WellKnownObjectMode.Singleton);
            return tcpSessionChannel;
        }

        public static TcpChannel InitiateRemotingApiUserService()
        {
            var port = Int32.Parse(SettingsMgr.ReadSetting(ServerConfig.ApiUserServicePort));
            var apiUserServiceChannel = (TcpChannel)GetChannel("apiUserChannel", port, false);
            ChannelServices.RegisterChannel(apiUserServiceChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ApiUserService),
                "ApiUserService",
                WellKnownObjectMode.Singleton);
            return apiUserServiceChannel;
        }

        public static TcpChannel InitiateRemotingMovieService()
        {
            var port = Int32.Parse(SettingsMgr.ReadSetting(ServerConfig.MovieServicePort));

            var movieServiceChannel = (TcpChannel)GetChannel("movieServiceChannel", port, false);
            ChannelServices.RegisterChannel(movieServiceChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MovieRemotingService),
                "movieService",
                WellKnownObjectMode.Singleton);
            return movieServiceChannel;
        }

        public static TcpChannel InitiateLogService()
        {
            var port = Int32.Parse(SettingsMgr.ReadSetting(ServerConfig.LogServicePort));
            var logServiceChannel = (TcpChannel)GetChannel("logServiceChannel", port, false);
            ChannelServices.RegisterChannel(logServiceChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemotingLogService),
                "logService",
                WellKnownObjectMode.Singleton);
            return logServiceChannel;
        }


        public static IChannel GetChannel(string name, int tcpPort, bool isSecure)
        {
            BinaryServerFormatterSinkProvider serverProv =
                new BinaryServerFormatterSinkProvider
                {
                    TypeFilterLevel = TypeFilterLevel.Full
                };
            IDictionary propBag = new Hashtable
            {
                ["port"] = tcpPort,
                ["typeFilterLevel"] = TypeFilterLevel.Full,
                ["name"] = name
            };
            if (isSecure)
            {
                propBag["secure"] = isSecure;
                propBag["impersonate"] = false;
            }
            return new TcpChannel(
                propBag, null, serverProv);
        }

    }
}
