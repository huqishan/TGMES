using Communication.Enum;
using Communication.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class RPCClient
    {
        #region Propertys
        private readonly IConnection _Connection;
        private readonly IModel _Channel;
        private readonly string _ReplyQueueName;
        private readonly EventingBasicConsumer _Consumer;
        private readonly BlockingCollection<string> _RespQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _Props;
        #endregion
        #region 构造
        public RPCClient(string rabbitIp, string userName, string passWord)
        {
            var factory = new ConnectionFactory() { HostName = rabbitIp, UserName = userName, Password = passWord };
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
                    _RespQueue.Add(response);
                }
            };
            _Channel.BasicConsume(queue: _ReplyQueueName, true, _Consumer);
        }
        #endregion
        #region 方法
        public bool DisConnect()
        {
            _Connection?.Close();
            _Connection?.Dispose();
            return true;
        }
        public string Send(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _Channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: _Props, body: messageBytes);
            return _RespQueue.Take();
        }
        public MesResult AlarmOccurred(List<MesDataInfoItem> mesData, string productID, string mothedName = "Alarm")
        {
            return GetResult(mesData, productID, MothedType.ALARM, mothedName);
        }
        public MesResult StateChanged(List<MesDataInfoItem> mesData, string productID, string mothedName = "StateChange")
        {
            return GetResult(mesData, productID, MothedType.STATECHANGED, mothedName);
        }
        public MesResult StationIn(List<MesDataInfoItem> mesData, string productID, string mothedName = "StationIn")
        {
            return GetResult(mesData, productID, MothedType.STATIONIN, mothedName);
        }
        public MesResult StationOut(List<MesDataInfoItem> mesData, string productID, string mothedName = "StationOut")
        {
            return GetResult(mesData, productID, MothedType.STATIONOUT, mothedName);
        }
        public MesResult UpLoadTestData(List<MesDataInfoItem> mesData, string productID, string mothedName = "TestData")
        {
            return GetResult(mesData, productID, MothedType.TESTDATA, mothedName);
        }
        public MesResult UserLogin(List<MesDataInfoItem> mesData, string productID, string mothedName = "UserLogin")
        {
            return GetResult(mesData, productID, MothedType.USERLOGIN, mothedName);
        }
        public MesResult UserLogOut(List<MesDataInfoItem> mesData, string productID, string mothedName = "UserLogOut")
        {
            return GetResult(mesData, productID, MothedType.USERLOGOUT, mothedName);
        }
        private MesResult GetResult(List<MesDataInfoItem> mesData, string productID, MothedType mothedType, string mothedName)
        {
            MesDataInfoTree model = new MesDataInfoTree(productID, mesData, MothedType.USERLOGOUT, mothedName);
            string data = JsonConvert.SerializeObject(model);
            MesResult result = JsonConvert.DeserializeObject<MesResult>(Send(data));
            return result;
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


        ~RPCClient()
        {

            Dispose(false); //必须为false
        }
        #endregion
    }
}
