using Domain;
using IServices;
using System;

namespace Services.LogServices
{
    public class LoggerAssist
    {
        public void EventCreator(string commandType, string action)
        {
            ISenderService logManager = new SenderService();
            ServerEvent eventToLog = new ServerEvent
            {
                EventType = commandType,
                LogBody = action,
                Time = DateTime.Now
            };
            logManager.CreateApiLog(eventToLog);
        }

    }
}
