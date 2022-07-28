using Shared.Abstractions;
using Shared.Abstractions.Enum;
using Shared.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPSocket;
using HPSocket.Udp;
using Shared.Infrastructure.CommunicationProtocol.Model;
using Shared.Infrastructure.CommunicationProtocol.Enum;
using System.Text.RegularExpressions;
using System.Threading;

namespace Shared.Infrastructure.CommunicationProtocol
{
    internal class UDP : ICommunicationProtocol, IDisposable
    {
        #region Propertys
        /// <summary>
        /// 远程连接IP
        /// </summary>
        public string RemoteAddress { get; private set; } = "127.0.0.1";
        /// <summary>
        /// 远程端口
        /// </summary>
        public ushort RemotePort { get; private set; } = 5555;
        /// <summary>
        /// 本地名称
        /// </summary>
        public string LocalName { get; private set; } = string.Empty;
        Thread ClientThread = null;
        public ConnectStatus IsConnected
        {
            get
            {
                return _IsConnected;
            }
        }
        public IUdpClient UdpClient { get; private set; } = new UdpClient();
        private ConnectStatus _IsConnected { get; set; } = ConnectStatus.DisConnected;
        #endregion

        #region 构造
        public UDP(CommuniactionConfigModel config)
        {
            if (string.IsNullOrEmpty(config.LocalIPAddress))
                UdpClient.BindAddress = config.LocalIPAddress;
            if (config.LocalPort > 0)
                UdpClient.BindPort = config.LocalPort;
            RemoteAddress = config.RemoteIPAddress;
            RemotePort = config.RemotePort;
            LocalName = config.LocalName;
            EventInitial();
        }
        #endregion

        #region 事件
        public event ReceiveData OnReceive;
        public event Action<LogMessageModel> OnLog;

        /// <summary>
        /// 事件初始化
        /// </summary>
        private void EventInitial()
        {
            UdpClient.OnClose -= Udp_OnClose;//断开事件
            UdpClient.OnClose += Udp_OnClose;//断开事件
            UdpClient.OnConnect -= UdpClient_OnConnect;//客户端已连接事件
            UdpClient.OnConnect += UdpClient_OnConnect;//客户端已连接事件
            UdpClient.OnPrepareConnect -= UdpClient_OnPrepareConnect;//客户端正在连接事件
            UdpClient.OnPrepareConnect += UdpClient_OnPrepareConnect;//客户端正在连接事件
            UdpClient.OnReceive -= UdpClient_OnReceive;//接收服务器发送的数据事件
            UdpClient.OnReceive += UdpClient_OnReceive;//接收服务器发送的数据事件
            UdpClient.OnSend -= UdpClient_OnSend;//客户端发送数据事件
            UdpClient.OnSend += UdpClient_OnSend;//客户端发送数据事件
        }
        #region 与服务器断开连接事件
        /// <summary>
        /// 与服务器断开连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="socketOperation"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private HandleResult Udp_OnClose(HPSocket.IClient sender, SocketOperation socketOperation, int errorCode)
        {
            OnLog?.BeginInvoke(new LogMessageModel { Message = $"与服务器({RemoteAddress}:{RemotePort}) 断开连接！", Type = LogType.WARN }, null, null);
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
        private HandleResult UdpClient_OnConnect(HPSocket.IClient sender)
        {
            OnLog?.BeginInvoke(new LogMessageModel { Message = $"连接服务器({RemoteAddress}:{RemotePort}) 成功！", Type = LogType.INFO }, null, null);
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
        private HandleResult UdpClient_OnPrepareConnect(IClient sender, IntPtr socket)
        {
            _IsConnected = ConnectStatus.Connecting;
            OnLog?.BeginInvoke(new LogMessageModel { Message = $"正在连接服务器({RemoteAddress}:{RemotePort})........", Type = LogType.INFO }, null, null);
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
        private HandleResult UdpClient_OnReceive(HPSocket.IClient sender, byte[] data)
        {
            string[] Commands = OnReceiveHandler(data);
            foreach (var item in Commands)
            {
                OnLog?.BeginInvoke(new LogMessageModel { Message = $"服务器({RemoteAddress}:{RemotePort})-->{LocalName}:{item}", Type = LogType.INFO }, null, null);
                OnReceive?.Invoke(item, sender.ConnectionId,RemoteAddress, RemotePort);
            }
            return HandleResult.Ok;
        }

        public virtual string[] OnReceiveHandler(byte[] data)
        {
            return new string[] { Encoding.Default.GetString(data) };
        }
        #endregion

        #region  发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HandleResult UdpClient_OnSend(HPSocket.IClient sender, byte[] data)
        {
            string Command = OnSendHandler(data);
            OnLog?.BeginInvoke(new LogMessageModel { Message = $"{LocalName}-->服务器({RemoteAddress}:{RemotePort}) : {Command}", Type = LogType.INFO }, null, null);
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

        #region 方法
        public bool Close()
        {
            bool result = false;
            try
            {
                if (UdpClient != null)
                {
                    ClientThread.Abort();
                    result = UdpClient.Stop();
                }
            }
            catch (Exception ex)
            {
                OnLog?.BeginInvoke(new LogMessageModel { Message = $"{LocalName} UDP Stop Exception:{ex.Message}", Type = LogType.ERROR }, null, null);
            }
            return result;
        }
        public bool Read(ref ReadWriteModel readWriteModel)
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            bool result = false;
            try
            {
                if (CheckIpAddressAndPort(RemoteAddress, RemotePort.ToString()))//Check IP 及Port是否正确
                {

                    result = this.UdpClient.Connect(RemoteAddress, RemotePort);
                    ClientThread = new Thread(() =>
                    {
                        while (true)
                        {
                            if (!UdpClient.IsConnected)
                                UdpClient.Connect();
                            Thread.Sleep(2000);
                        }
                    })
                    { IsBackground = true };
                    ClientThread.Start();
                }
                else
                {
                    OnLog?.BeginInvoke(new LogMessageModel { Message = $"{LocalName} UDP Address or Port Error({RemoteAddress }:{RemotePort})", Type = LogType.ERROR }, null, null);
                }
            }
            catch (Exception ex)
            {
                OnLog?.BeginInvoke(new LogMessageModel { Message = $"{LocalName} UDP Connect Exception:{ex.Message}", Type = LogType.ERROR }, null, null);
                result = false;
            }
            return result;
        }

        public bool Write(ref ReadWriteModel readWriteModel,bool isWait=false)
        {
            byte[] data = Encoding.UTF8.GetBytes(readWriteModel.Message);
            return this.UdpClient.Send(data, data.Length);
        }

        public Task<bool> WriteAsync(ReadWriteModel readWriteModel)
        {
            return Task<bool>.Run(() => { return Write(ref readWriteModel); });
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
        public void Dispose()
        {
            Close();
            this.UdpClient?.Dispose();
        }
        #endregion
    }
}
