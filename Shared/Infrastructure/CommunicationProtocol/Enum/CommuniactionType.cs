using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol.Enum
{
    public enum CommuniactionType
    {
        TCPClient,
        TCPServer,
        MX,
        UDP,
        CAN,
        RabbitMQRPCServer,
        RabbitMQRPCClient
    }
}
