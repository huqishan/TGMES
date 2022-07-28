using Shared.Abstractions.Model;
using Shared.Infrastructure.CommunicationProtocol.Enum;
using Shared.Infrastructure.CommunicationProtocol.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol
{
    public delegate string ReceiveData(object message,params object[] param);
    /// <summary>
    /// 通讯协议对象（如果是通讯协议则需要继承此接口）
    /// </summary>
    public interface ICommunicationProtocol
    {
        /// <summary>
        /// 自定义消息处理事件
        /// </summary>
        event ReceiveData OnReceive;
        /// <summary>
        /// Log打印
        /// </summary>
        event Action<LogMessageModel> OnLog;
        /// <summary>
        /// 连接状态
        /// </summary>
        ConnectStatus IsConnected { get; }
        /// <summary>
        /// 开启
        /// </summary>
        /// <returns></returns>
        bool Start();
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="receiveObj">接收者对象</param>
        /// <returns></returns>
        bool Write(ref ReadWriteModel readWriteModel,bool isWait= false);
        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="receiveObj">接收者对象</param>
        /// <returns></returns>
        Task<bool> WriteAsync(ReadWriteModel readWriteModel);
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="readObj">读取对象（string/string[]）</param>
        /// <param name="lenght">长度</param>
        /// <param name="type">数据格式</param>
        /// <returns>string/string[]</returns>
        bool Read(ref ReadWriteModel readWriteModel);
        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        bool Close();
    }
}
