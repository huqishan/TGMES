using Shared.Infrastructure.CommunicationProtocol.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol.Model
{
    public class ReadWriteModel
    {
        /// <summary>
        /// TCP Client/UDP
        /// </summary>
        /// <param name="message"></param>
        public ReadWriteModel(string message)
        {
            this.Message = message;
        }
        /// <summary>
        /// TCP Server
        /// </summary>
        /// <param name="message"></param>
        /// <param name="recciveObj"></param>
        public ReadWriteModel(string message, object recciveObj)
        {
            this.Message = message;
            this.ClientId = recciveObj;
        }
        /// <summary>
        /// MX
        /// </summary>
        /// <param name="message"></param>
        /// <param name="plcAddress"></param>
        /// <param name="lenght"></param>
        /// <param name="type"></param>
        public ReadWriteModel(string message, object plcAddress, int lenght, DataType type = DataType.Decimal)
        {
            this.Message = message;
            this.PLCAddress = plcAddress;
            this.Lenght = lenght;
            this.Type = type;
        }
        public string Message { get; private set; }
        public object ClientId { get; private set; }
        public object PLCAddress { get; private set; }
        public int Lenght { get; private set; }
        public DataType Type { get; private set; }
        /// <summary>
        /// 反馈结果
        /// </summary>
        public object Result { get; set; }
    }
}
