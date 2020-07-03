using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAdmin
{
    public class ApiConfig
    {
        public static string SessionServiceIp = "tcp://127.0.0.1:8500/SessionService";
        public static string ApiUserServiceIp = "tcp://127.0.0.1:6969/ApiUserService";
        public static string MovieServiceIp = "tcp://127.0.0.1:4200/movieService";
        public static string LogServiceIP = "tcp://127.0.0.1:4201/logService";
    }
}
