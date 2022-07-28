using HPSocket;
using Shared.Abstractions;
using Shared.Abstractions.Enum;
using Shared.Abstractions.Model;
using Shared.Infrastructure.CommunicationProtocol.Enum;
using Shared.Infrastructure.CommunicationProtocol.Model;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol
{
    public class TCPClient : ICommunicationProtocol, IDisposable
    {
        #region Propertys
        /// <summary>
        /// 远程服务器IP 地址
        /// </summary>
        public string RemoteAddress { get; private set; } = "127.0.0.1";

        /// <summary>
        /// 远程服务器端口
        /// </summary>
        public ushort RemotePort { get; private set; } = 5555;

        /// <summary>
        /// TCP 客户端
        /// </summary>
        public ITcpClient TcpClient { get; private set; } = new HPSocket.Tcp.TcpClient();

        /// <summary>
        /// TCP 客户端名字
        /// </summary>
        public string LocalClientName { get; private set; } = string.Empty;
        Thread ClientThread;
        AutoResetEvent IsWhile = new AutoResetEvent(false);
        AutoResetEvent IsReceive = new AutoResetEvent(true);
        /// <summary>
        /// 缓冲区
        /// </summary>
        StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 预留
        /// </summary>
        public object Tag { get; private set; }
        private ConnectStatus _IsConnected { get; set; } = ConnectStatus.DisConnected;
        /// <summary>
        /// TCP 客户端连接状态
        /// </summary>  
        public ConnectStatus IsConnected
        {
            get
            {
                return _IsConnected;
            }
        }
        #endregion

        #region 构造
        public TCPClient(CommuniactionConfigModel config)
        {
            if (string.IsNullOrEmpty(config.LocalIPAddress))
                TcpClient.BindAddress = config.LocalIPAddress;
            if (config.LocalPort > 0)
                TcpClient.BindPort = config.LocalPort;
            RemoteAddress = config.RemoteIPAddress;
            RemotePort = config.RemotePort;
            LocalClientName = config.LocalName;
            EventInitial();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            bool result = false;
            try
            {
                TcpClient.KeepAliveInterval = 2000;
                TcpClient.KeepAliveTime = 2000;
                if (CheckIpAddressAndPort(RemoteAddress, RemotePort.ToString()))//Check IP 及Port是否正确
                {
                    IsWhile.Reset();
                    result = TcpClient.Connect(RemoteAddress, RemotePort);
                    ClientThread = new Thread(() =>
                    {
                        while (true)
                        {
                            if (!TcpClient.IsConnected)
                                TcpClient.Connect();
                            if (IsWhile.WaitOne(2000))
                                break;
                        }
                    })
                    { IsBackground = true };
                    ClientThread.Start();
                }
                else
                {
                    WriteLog(new LogMessageModel { Message = $"{LocalClientName} TCP Address or Port Error({RemoteAddress }:{RemotePort})", Type = LogType.ERROR });
                }
            }
            catch (Exception ex)
            {
                WriteLog(new LogMessageModel { Message = $"{LocalClientName} TCP Connect Exception:{ex.Message}", Type = LogType.ERROR });
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 断开服务器
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool result = false;
            try
            {
                if (TcpClient != null)
                {
                    IsWhile.Set();
                    result = TcpClient.Stop();
                }
            }
            catch (Exception ex)
            {
                WriteLog(new LogMessageModel { Message = $"{LocalClientName} TCP Stop Exception:{ex.Message}", Type = LogType.ERROR });
            }
            return result;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="ReceiveId">接收方对象</param>
        /// <returns></returns>
        public bool Write(ref ReadWriteModel readWriteModel, bool isWait = false)
        {
            if (isWait)
            {
                lock (IsReceive)
                {
                    IsReceive.WaitOne();
                }
                IsReceive.Reset();
            }
            bool result = true;
            if (!string.IsNullOrEmpty(readWriteModel.Message) && TcpClient != null && TcpClient.IsConnected)
            {
                byte[] data = Encoding.UTF8.GetBytes(readWriteModel.Message);
                result=TcpClient.Send(data, 0, data.Length);
                
                if (isWait)
                {
                    if(IsReceive.WaitOne(10000))
                    {
                        readWriteModel.Result = sb.ToString(); 
                    }
                    else
                    {
                        readWriteModel.Result = "Wait TimeOut";
                        WriteLog(new LogMessageModel { Message = $"{LocalClientName} Wait TimeOut:{readWriteModel.Message},TcpClient:{TcpClient == null},TcpClientIsConnect:{TcpClient.IsConnected} ", Type = LogType.ERROR });
                        result = false;
                    }
                }
                sb.Clear();
                IsReceive.Set();
            }
            else
            {
                readWriteModel.Result = $"{LocalClientName} TCP Connect Exception Command:{readWriteModel.Message},TcpClient:{TcpClient == null},TcpClientIsConnect:{TcpClient.IsConnected} ";
                WriteLog(new LogMessageModel { Message = $"{LocalClientName} TCP Connect Exception Command:{readWriteModel.Message},TcpClient:{TcpClient == null},TcpClientIsConnect:{TcpClient.IsConnected} ", Type = LogType.ERROR });
                result = false;
                //发送失败
            }
            return result;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="ReceiveId">接收方对象</param>
        public Task<bool> WriteAsync(ReadWriteModel readWriteModel)
        {
            return Task.Run(() => {return Write(ref readWriteModel);});
        }
        public bool Read(ref ReadWriteModel readWriteModel)
        {
            return true;
        }
        /// <summary>
        /// Check IP 及Port是否正确
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckIpAddressAndPort(string ip, string port)
        {
            bool result = false;
            try
            {
                if (Regex.IsMatch(ip + ":" + port, @"^((2[0-4]\d|25[0-5]|[1]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[1]?\d\d?)\:([1-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]|[1-6][0-5][0-5][0-3][0-5])$"))
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region 事件
        public event Action<LogMessageModel> OnLog;
        public event ReceiveData OnReceive;
        private void WriteLog(LogMessageModel message)
        {
            Task.Run(() => { OnLog?.Invoke(message); });
        }
        /// <summary>
        /// 事件初始化
        /// </summary>
        private void EventInitial()
        {
            TcpClient.OnClose -= TcpClient_OnClose;//客户端断开事件
            TcpClient.OnClose += TcpClient_OnClose;//客户端断开事件
            TcpClient.OnConnect -= TcpClient_OnConnect;//客户端已连接事件
            TcpClient.OnConnect += TcpClient_OnConnect;//客户端已连接事件
            TcpClient.OnPrepareConnect -= TcpClient_OnPrepareConnect;//客户端正在连接事件
            TcpClient.OnPrepareConnect += TcpClient_OnPrepareConnect;//客户端正在连接事件
            TcpClient.OnReceive -= TcpClient_OnReceive;//接收服务器发送的数据事件
            TcpClient.OnReceive += TcpClient_OnReceive;//接收服务器发送的数据事件
            TcpClient.OnSend -= TcpClient_OnSend;//客户端发送数据事件
            TcpClient.OnSend += TcpClient_OnSend;//客户端发送数据事件
        }

        #region 与服务器断开连接事件
        /// <summary>
        /// 与服务器断开连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="socketOperation"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private HandleResult TcpClient_OnClose(HPSocket.IClient sender, SocketOperation socketOperation, int errorCode)
        {
            WriteLog(new LogMessageModel { Message = $"{LocalClientName} 与服务器({RemoteAddress}:{RemotePort}) 断开连接！", Type = LogType.WARN });
            _IsConnected = ConnectStatus.DisConnected;
            return HandleResult.Ok;
        }
        #endregion

        #region 已经连接服务器事件
        /// <summary>
        /// 已经连接服务器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private HandleResult TcpClient_OnConnect(HPSocket.IClient sender)
        {
            WriteLog(new LogMessageModel { Message = $"{LocalClientName} 连接服务器({RemoteAddress}:{RemotePort}) 成功！", Type = LogType.INFO });
            _IsConnected = ConnectStatus.Connected;
            return HandleResult.Ok;
        }
        #endregion

        #region 正在连接服务器事件
        /// <summary>
        ///正在连接服务器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        private HandleResult TcpClient_OnPrepareConnect(IClient sender, IntPtr socket)
        {
            _IsConnected = ConnectStatus.Connecting;
            WriteLog(new LogMessageModel { Message = $"{LocalClientName} 正在连接服务器({RemoteAddress}:{RemotePort})........", Type = LogType.INFO });
            return HandleResult.Ok;
        }
        #endregion

        #region  接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HandleResult TcpClient_OnReceive(HPSocket.IClient sender, byte[] data)
        {
            string[] Commands = OnReceiveHandler(data);

            foreach (var item in Commands)
            {
                sb.Append(item);
                IsReceive.Set();
                WriteLog(new LogMessageModel { Message = $"服务器{RemoteAddress}:{RemotePort}-->{LocalClientName}:{item}", Type = LogType.INFO });
                Task.Run(() =>
                {
                    OnReceive?.Invoke(item,sender.ConnectionId,RemoteAddress,RemotePort);
                });
            }
            return HandleResult.Ok;
        }

        public virtual string[] OnReceiveHandler(byte[] data)
        {
            return new string[] { Encoding.UTF8.GetString(data) };
        }
        #endregion

        #region  发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HandleResult TcpClient_OnSend(HPSocket.IClient sender, byte[] data)
        {
            string command = OnSendHandler(data);
            WriteLog(new LogMessageModel { Message = $"{LocalClientName}-->服务器({RemoteAddress}:{RemotePort}) : {command}", Type = LogType.INFO });
            return HandleResult.Ok;
        }


        /// <summary>
        /// 发送数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual string OnSendHandler(byte[] data)
        {
            string result = "";
            try
            {
                result = Encoding.Default.GetString(data);
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion
        #endregion

        #region Dispose
        private bool disposed = false;
        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                try
                {
                    if (TcpClient != null)
                    {
                        Close();
                        TcpClient.Dispose();
                    }
                }
                catch (Exception)
                {

                }

            }
            //让类型知道自己已经被释放
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true); //必须为true
            GC.SuppressFinalize(this);  //通知垃圾回收机制不再调用终结器（析构器）
        }


        ~TCPClient()
        {

            Dispose(false); //必须为false
        }
        #endregion
    }
}
