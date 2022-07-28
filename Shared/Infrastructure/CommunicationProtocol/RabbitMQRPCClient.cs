using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Abstractions.Model;
using Shared.Infrastructure.CommunicationProtocol.Enum;
using Shared.Infrastructure.CommunicationProtocol.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.CommunicationProtocol
{
    public class RabbitMQRPCClient : ICommunicationProtocol, IDisposable
    {
        #region Propertys
        private readonly IConnection _Connection;
        private readonly IModel _Channel;
        private readonly string _ReplyQueueName;
        private readonly EventingBasicConsumer _Consumer;
        private readonly BlockingCollection<string> _RespQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _Props;
        public ConnectStatus IsConnected
        {
            get
            {
                if (_Connection != null)
                    return _Connection.IsOpen ? ConnectStatus.Connected : ConnectStatus.DisConnected;
                return ConnectStatus.DisConnected;
            }
        }
        #endregion
        #region 构造
        public RabbitMQRPCClient(CommuniactionConfigModel config)
        {
            var factory = new ConnectionFactory() { HostName = config.RemoteIPAddress, UserName = config.UserName, Password = config.PassWord };
            _Connection = factory.CreateConnection();

            _Channel = _Connection.CreateModel();
            _ReplyQueueName = _Channel.QueueDeclare().QueueName;
            _Consumer = new EventingBasicConsumer(_Channel);
            _Props = _Channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _Props.CorrelationId = correlationId;
            _Props.ReplyTo = _ReplyQueueName;
            _Consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    WriteLog(new LogMessageModel() { Message = $"接收到Server反馈：{response}", Type=Abstractions.Enum.LogType.INFO});
                    _RespQueue.Add(response);
                }
            };
            _Channel.BasicConsume(consumer: _Consumer, queue: _ReplyQueueName, autoAck: true);
        }
        #endregion
        #region 方法
        public bool Close()
        {
            _Connection?.Close();
            _Connection?.Dispose();
            return true;
        }

        public bool Read(ref ReadWriteModel readWriteModel)
        {
            return true;
        }

        public bool Start()
        {
            return true;
        }

        public bool Write(ref ReadWriteModel readWriteModel, bool isWait = false)
        {
            var messageBytes = Encoding.UTF8.GetBytes(readWriteModel.Message);
            _Channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: _Props, body: messageBytes);
            if (isWait)
            {
                readWriteModel.Result = _RespQueue.Take();
            }
            else
            {
                OnReceive?.Invoke(_RespQueue.Take());
            }
            return true;
        }

        public Task<bool> WriteAsync(ReadWriteModel readWriteModel)
        {
            return Task.FromResult(Write(ref readWriteModel));
        }
        #endregion
        #region 事件
        public event ReceiveData OnReceive;
        public event Action<LogMessageModel> OnLog;
        private void WriteLog(LogMessageModel message)
        {
            Task.Run(() => { OnLog?.Invoke(message); });
        }
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

            }
            //让类型知道自己已经被释放
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true); //必须为true
            GC.SuppressFinalize(this);  //通知垃圾回收机制不再调用终结器（析构器）
        }


        ~RabbitMQRPCClient()
        {

            Dispose(false); //必须为false
        }
        #endregion
    }
}
