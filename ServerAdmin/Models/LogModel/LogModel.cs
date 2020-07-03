using Domain;
using System;

namespace ServerAdmin.Models
{
    public class LogModel
    {
        public string EventType { get; set; }
        public string LogBody { get; set; }
        public DateTime Time { get; set; }


        public LogModel(ServerEvent e)
        {
            EventType = e.EventType;
            LogBody = e.LogBody;
            Time = e.Time;
        }

        public override string ToString()
        {
            return $"Log from {Time.Date} at {Time.Hour}: {EventType} {LogBody}";
        }


    }
}