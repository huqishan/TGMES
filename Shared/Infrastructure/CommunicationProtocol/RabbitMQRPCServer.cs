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
    public class RabbitMQRPCServer : ICommunicationProtocol, IDisposable
    {
        #region Propertys
        private readonly IConnection _Connection;
        private readonly IModel _Channel;
        private readonly EventingBasicConsumer _Consumer;

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
        public RabbitMQRPCServer(CommuniactionConfigModel config)
        {
            var factory = new ConnectionFactory() { HostName = config.RemoteIPAddress, Port=config.RemotePort,UserName = config.UserName, Password = config.PassWord };
            _Connection = factory.CreateConnection();
            _Channel = _Connection.CreateModel();
            _Channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _Channel.BasicQos(0, 1, false);
            _Consumer = new EventingBasicConsumer(_Channel);
            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += (model, ea) =>
            {
                string? response = null;
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = _Channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                try
                {
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    WriteLog(new LogMessageModel() { Message = $"ClientId:{props.CorrelationId}-->Server:{message}", Type = Abstractions.Enum.LogType.INFO });
                    response = OnReceive?.Invoke(message, props.CorrelationId);
                }
                catch (Exception e)
                {
                    WriteLog(new LogMessageModel() { Message = $"处理RabbitMQ消息失败：{e.Message}", Type = Abstractions.Enum.LogType.ERROR });
                }
                finally
                {
                    if (response != null) 
                    {
                        WriteLog(new LogMessageModel() { Message = $"Server-->ClientId:{replyProps.CorrelationId}:{response}", Type = Abstractions.Enum.LogType.INFO });
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        _Channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        _Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
            };
            _Channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
            WriteLog(new LogMessageModel() { Message = $"RabbitMQ 通讯成功！", Type = Abstractions.Enum.LogType.INFO });
        }
        #endregion
        #region 方法
        public bool Close()
        {
            _Connection?.Close();
            _Connection?.Dispose();
            WriteLog(new LogMessageModel() { Message = $"RabbitMQ 通讯断开成功！", Type = Abstractions.Enum.LogType.INFO });
            return true;
        }

        public bool Read(ref ReadWriteModel readWriteModel)
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            return true;
        }

        public bool Write(ref ReadWriteModel readWriteModel, bool isWait = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteAsync(ReadWriteModel readWriteModel)
        {
            throw new NotImplementedException();
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


        ~RabbitMQRPCServer()
        {

            Dispose(false); //必须为false
        }
        #endregion
    }
}
