using System;

namespace Domain
{
    [Serializable]
    public class ServerEvent
    {
        public string EventType { get; set; }
        public string LogBody { get; set; }
        public DateTime Time { get; set; }
    }
}
