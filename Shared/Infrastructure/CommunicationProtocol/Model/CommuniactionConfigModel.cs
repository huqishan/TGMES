using Shared.Infrastructure.CommunicationProtocol.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol.Model
{
    public class CommuniactionConfigModel
    {
        /// <summary>
        /// TCPServer
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="localIpAddress"></param>
        /// <param name="LocalPort"></param>
        public CommuniactionConfigModel(string localName, string localIpAddress, ushort localPort)
        {
            this.LocalName = localName;
            this.LocalIPAddress = localIpAddress;
            this.LocalPort = localPort;
            this.Type = CommuniactionType.TCPServer;
        }
        /// <summary>
        /// TCPClient/UDP
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="remoteIpAddress"></param>
        /// <param name="remotePort"></param>
        /// <param name="localIpAddress"></param>
        /// <param name="localPort"></param>
        public CommuniactionConfigModel(bool isUdp, string localName, string remoteIpAddress, ushort remotePort, string localIpAddress = null, ushort localPort = 0)
        {
            this.LocalName = localName;
            this.RemoteIPAddress = remoteIpAddress;
            this.RemotePort = remotePort;
            this.LocalIPAddress = localIpAddress;
            this.LocalPort = localPort;
            this.Type = isUdp ? CommuniactionType.UDP : CommuniactionType.TCPClient;
        }
        /// <summary>
        /// MX
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="plcActLogicalStationNumber"></param>
        /// <param name="passWord"></param>
        public CommuniactionConfigModel(string localName, int plcActLogicalStationNumber, string passWord = null)
        {
            this.LocalName = localName;
            this.PLCActLogicalStationNumber = plcActLogicalStationNumber;
            this.PassWord = passWord;
            this.Type = CommuniactionType.MX;
        }
        /// <summary>
        /// CAN
        /// </summary>
        /// <param name="canType"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="baudRate"></param>
        /// <param name="channel"></param>
        public CommuniactionConfigModel(CANType canType, string ip, ushort port, string baudRate, int channel)
        {
            this.CANType = canType;
            RemoteIPAddress = ip;
            RemotePort = port;
            this.BaudRete = baudRate;
            this.Channel = channel;
            Type = CommuniactionType.CAN;
        }
        /// <summary>
        /// RabbitMQ
        /// </summary>
        /// <param name="canType"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="baudRate"></param>
        /// <param name="channel"></param>
        public CommuniactionConfigModel(bool isServer, string ip, ushort port, string userName, string passWord)
        {
            RemoteIPAddress = ip;
            RemotePort = port;
            this.UserName = userName;
            this.PassWord = passWord;
            this.LocalName = isServer ? "RabbitMQRPCServer" : "RabbitMQRPCClient";
            Type = isServer ? CommuniactionType.RabbitMQRPCServer : CommuniactionType.RabbitMQRPCClient;
        }
        public CommuniactionType Type { get; private set; }
        public string LocalName { get; private set; } = null;
        public string LocalIPAddress { get; private set; } = null;
        public ushort LocalPort { get; private set; } = 0;
        public string RemoteIPAddress { get; private set; } = null;
        public ushort RemotePort { get; private set; } = 0;
        public int PLCActLogicalStationNumber { get; private set; } = 0;
        public string PassWord { get; private set; } = null;
        public CANType CANType { get; private set; }
        public int Channel { get; private set; } = 0;
        public string BaudRete { get; private set; }
        public string UserName { get; private set; } = null;

    }
}
