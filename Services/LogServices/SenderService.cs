using Common;
using Common.Interfaces;
using Domain;
using QueueStarter;
using System;
using System.Collections.Generic;
using System.Messaging;
using IServices;

namespace Services
{
    public class SenderService : ISenderService 
    {

        private static MessageQueue queue;

        public SenderService()
        {
            queue = new MessageQueue(PathConfig.PATH) { Formatter = new XmlMessageFormatter(new[] { typeof(ServerEvent) }) };
        }

        public void CreateApiLog(ServerEvent e)
        {
            ServerEvent eventToSerialize = new ServerEvent
            {
                EventType = e.EventType,
                LogBody = e.LogBody,
                Time = e.Time
            };
            var msg = new Message(eventToSerialize)
            {
                Label = $"ServerLog from {eventToSerialize.Time}"
            };
            queue.Send(msg);
        }

        public void CreateLog(string command, byte[] frame, int size)
        {
            IParser parser = new Parser();
            var data = parser.GetDataObject(frame);
            string msgToLog = "";
            for (int i = 0; i < size; i++)
            {
                msgToLog += data[i];
            }
            ServerEvent eventToSerialize = new ServerEvent
            {
                EventType = command,
                LogBody = msgToLog,
                Time = DateTime.Now
            };
            var msg = new Message(eventToSerialize)
            {
                Label = $"ServerLog from {eventToSerialize.Time}"
            };
            queue.Send(msg);
        }

        public List<ServerEvent> GetFromMsMq(string filter)
        {
            var messages = queue.GetAllMessages();
            var serverEvents = new List<ServerEvent>();
            if (filter.Length == 0)
            {
                foreach (var m in messages)
                {
                    var serverE = m.Body as ServerEvent;
                    serverEvents.Add(serverE);
                }

            }
            else
            {
                foreach (var m in messages)
                {
                    var serverE = m.Body as ServerEvent;
                    if (serverE.EventType.Equals(filter))
                    {
                        serverEvents.Add(serverE);
                    }
                }
            }

            return serverEvents;
        }



    }

}
