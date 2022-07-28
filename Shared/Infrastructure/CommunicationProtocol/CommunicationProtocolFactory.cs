using Shared.Abstractions;
using Shared.Abstractions.Enum;
using Shared.Abstractions.Model;
using Shared.Infrastructure.CommunicationProtocol.Enum;
using Shared.Infrastructure.CommunicationProtocol.Model;
using System.Collections.Generic;

namespace Shared.Infrastructure.CommunicationProtocol
{
    public class CommunicationProtocolFactory
    {
        private static Dictionary<string, ICommunicationProtocol> keyValuePairs = new Dictionary<string, ICommunicationProtocol>();
        public static ICommunicationProtocol? CreateCommuniactionProtocol(CommuniactionConfigModel config)
        {
            ICommunicationProtocol? communiaction = null;
            switch (config.Type)
            {
                case CommuniactionType.TCPClient:
                    communiaction = new TCPClient(config);
                    break;
                case CommuniactionType.TCPServer:
                    communiaction = new TCPServer(config);
                    break;
                case CommuniactionType.UDP:
                    communiaction = new UDP(config);
                    break;
                case CommuniactionType.CAN:
                    communiaction = new CAN(config);
                    break;
                case CommuniactionType.RabbitMQRPCServer:
                    communiaction = new RabbitMQRPCServer(config);
                    break;
                case CommuniactionType.RabbitMQRPCClient:
                    communiaction = new RabbitMQRPCClient(config);
                    break;
                default:
                    break;
            }

            if (keyValuePairs.ContainsKey(config.LocalName))
            {
                keyValuePairs[config.LocalName].Close();
                keyValuePairs[config.LocalName] = communiaction;
            }
            else
                keyValuePairs.Add(config.LocalName, communiaction);
            return communiaction;

        }
        public static ICommunicationProtocol Get(string name)
        {
            ICommunicationProtocol communication = null;
            if (keyValuePairs.ContainsKey(name))
            {
                communication = keyValuePairs[name];
            }
            return communication;
        }
        public static bool Remove(string name)
        {
            if (keyValuePairs.ContainsKey(name))
            {
                keyValuePairs[name].Close(); 
                keyValuePairs.Remove(name); 
            }
            return true;
        }
    }
}
