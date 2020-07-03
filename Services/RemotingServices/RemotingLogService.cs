using Domain;
using IServices;
using System;
using System.Collections.Generic;

namespace Services
{
    public class RemotingLogService : MarshalByRefObject, ILogService
    {
        public void CreateLog(ServerEvent log)
        {
            var logManager = new SenderService();
            logManager.CreateApiLog(log);
        }

        public List<ServerEvent> GetLog(string filter)
        {
            var logManager = new SenderService();
            return logManager.GetFromMsMq(filter);
        }
    }
}
