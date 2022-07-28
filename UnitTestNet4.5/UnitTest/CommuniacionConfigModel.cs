using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol.Model
{
    public class CommuniactionConfigModel
    {

        public CommuniactionConfigModel(CANType canType, string ip, ushort port, string baudRate, int channel)
        {
            this.CANType = canType;
            RemoteIPAddress = ip;
            RemotePort = port;
            this.BaudRete = baudRate;
            this.Channel = channel;
        }
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

    }
    public enum CANType
    {
        CANNET_2E_U = 17,
        ZCAN_USBCAN_2E_U = 21,
        ZCAN_USBCAN_E_U = 20
    }
}