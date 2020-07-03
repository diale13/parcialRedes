using System.Messaging;
using System;

namespace QueueStarter
{
    public class QueueStarter
    {
        public static void Main(string[] args)
        {
            string queuePath = PathConfig.PATH;
            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath);
            }
        }

    }
}
