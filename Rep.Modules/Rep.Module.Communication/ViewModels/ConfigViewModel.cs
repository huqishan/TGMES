using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Rep.Controls.Converts;
using Rep.Controls.CustomContorls;
using Rep.Controls.Enum;
using Rep.Controls.Events;
using Rep.Controls.Model;
using Rep.Module.Communication.PropertyVMs;
using Rep.Module.Communication.Services.Application.Commands;
using Shared.Infrastructure.CommunicationProtocol;
using Shared.Infrastructure.CommunicationProtocol.Model;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.PackMethod;
using System;
using System.Threading.Tasks;

namespace Rep.Module.Communication.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        #region Propertys
        #region Config 属性事件分离
        private ConfigPropertysVM _ConfigPropertys;

        public ConfigPropertysVM ConfigPropertys
        {
            get { return _ConfigPropertys; }
            set
            {
                if (value != _ConfigPropertys)
                {
                    SetProperty(ref _ConfigPropertys, value);
                }
            }
        }
        #endregion 
        #region DataConfig
        private MESDataConfigControl _DataConfig;

        public MESDataConfigControl DataConfig
        {
            get { return _DataConfig; }
            set
            {
                if (value != _DataConfig)
                {
                    SetProperty(ref _DataConfig, value);
                }
            }
        }
        #endregion

        private string _Url;
        readonly string file = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ConnectConfig";
        ICommunicationProtocol ServerObj = null;
        ICommunicationProtocol MESObj = null;
        IEventAggregator _aggregator;
        IDialogService _dialogService;
        ILogger _logger;
        #endregion
        public ConfigViewModel(IEventAggregator aggregator, IDialogService dialogService, ILogger logger)
        {
            _aggregator = aggregator;
            _dialogService = dialogService;
            _logger = logger;
            Load();
            DataConfig = new MESDataConfigControl(_dialogService);
            _aggregator.GetEvent<UpLoadEvent>().Subscribe(UpLoadHandler, r => !r.IsResult);
        }
        #region Method
        private void Load()
        {
            ConfigPropertys = JsonHelper.ReadJson<ConfigPropertysVM>($"{file}\\ConnectConfig.json") ?? new ConfigPropertysVM();
            HostListen();
        }
        private void UpLoadHandler(UpLoadMessageModel model)
        {
            foreach (var item in model.CommunicationDatas)
            {
                SendMES(item.ClientData, item.Id);
            }
            _aggregator.GetEvent<UpLoadEvent>().Publish(new UpLoadMessageModel { IsResult = true });
        }
        #region Save
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(SaveExecute);
        private void SaveExecute()
        {
            JsonHelper.SaveJson(ConfigPropertys, $"{file}\\ConnectConfig.json");
            Task.Run(() =>
            {
                HostListen();
            });
        }
        #endregion
        #region Convert
        private DelegateCommand _ConvertCommand;
        public DelegateCommand ConvertCommand => _ConvertCommand ??= new DelegateCommand(ConvertExecute);
        private void ConvertExecute()
        {
            MesDataInfoTree mesData = JsonConvert.DeserializeObject<MesDataInfoTree>(ConfigPropertys.SourceData);
            mesData.Url = ConfigPropertys.URL;
            mesData.Xmlns = ConfigPropertys.XMLNS;
            mesData.Xsi = ConfigPropertys.XSI;
            mesData.Xsd = ConfigPropertys.XSD;
            mesData.Soap = ConfigPropertys.SOAP;
            ConfigPropertys.ConvertedData = MesDataConvert.Convert(mesData, out _Url);
        }
        #endregion
        #region Send
        private DelegateCommand _SendCommand;
        public DelegateCommand SendCommand => _SendCommand ??= new DelegateCommand(SendExecute);
        private void SendExecute()
        {
            if (ConfigPropertys.SelectMESType.ToUpper().Contains("WEBSERVICES"))
            {
                ConfigPropertys.Result = WebServicesHelper.Send(ConfigPropertys.ConvertedData, _Url).ToXMLFormat();
            }
            else if (ConfigPropertys.SelectMESType.ToUpper().Contains("WEBAPI"))
            {
                ConfigPropertys.Result = WebApiHelper.Send(ConfigPropertys.ConvertedData, ConfigPropertys.URL).ToJsonFormat();
            }
            else if (ConfigPropertys.SelectMESType.ToUpper().Contains("CLIENT"))
            {
                ReadWriteModel write = new ReadWriteModel(ConfigPropertys.ConvertedData);
                MESObj.Write(ref write, true);
                ConfigPropertys.Result = write.Result.ToString();
            }
        }
        #endregion
        #region Listen
        private void HostListen()
        {
            try
            {
                MESObj?.Close();
                CommuniactionConfigModel config = new CommuniactionConfigModel(isServer: true, ConfigPropertys.RabbitMQAddress, ConfigPropertys.RabbitMQPort, ConfigPropertys.RabbitMQUserName, ConfigPropertys.RabbitMQPassWord);
                ServerObj = CommunicationProtocolFactory.CreateCommuniactionProtocol(config);
                ServerObj.OnReceive += ServerCommuniactionObj_OnReceive;
                ServerObj.OnLog += ServerCommuniactionObj_OnLog;
                ServerObj.Start();

                if (ConfigPropertys.SelectMESType.Contains("Client"))
                {
                    config = new CommuniactionConfigModel(false, "MES", ConfigPropertys.MESRemoteIpAddress, ConfigPropertys.MESRemotePort, ConfigPropertys.MESLocalIpAddress, ConfigPropertys.MESLoaclPort);
                    MESObj = CommunicationProtocolFactory.CreateCommuniactionProtocol(config);
                    MESObj.OnReceive += MESCommuniactionObj_OnReceive;
                    MESObj.OnLog += MESCommuniactionObj_OnLog;
                    MESObj.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"创建通讯失败：{ex.Message}");
            }

        }
        private string MESCommuniactionObj_OnReceive(object message, params object[] param)
        {
            return ConfigPropertys.Result = message.ToString();
        }
        private void MESCommuniactionObj_OnLog(Shared.Abstractions.Model.LogMessageModel obj)
        {
            _logger.LogInformation($"{obj.Message}");
            UIAction(() =>
            {
                if (ConfigPropertys.MESCommunicationData.Count > 10)
                    ConfigPropertys.MESCommunicationData.RemoveAt(0);
                ConfigPropertys.MESCommunicationData.Add($"{obj.Message}");
            });
        }

        private void ServerCommuniactionObj_OnLog(Shared.Abstractions.Model.LogMessageModel obj)
        {
            UIAction(() =>
            {
                if (ConfigPropertys.CommunicationData.Count > 10)
                    ConfigPropertys.CommunicationData.RemoveAt(0);
                ConfigPropertys.CommunicationData.Add($"{obj.LogTime:yyyy-MM-dd HH:mm:ss ffff} {obj.Message}");
            });
            switch (obj.Type)
            {
                case Shared.Abstractions.Enum.LogType.INFO:
                    _logger.LogInformation(obj.Message);
                    break;
                case Shared.Abstractions.Enum.LogType.DEBUG:
                    _logger.LogDebug(obj.Message);
                    break;
                case Shared.Abstractions.Enum.LogType.WARN:
                    _logger.LogWarning(obj.Message);
                    break;
                case Shared.Abstractions.Enum.LogType.ERROR:
                    _logger.LogError(obj.Message);
                    break;
                default:
                    break;
            }
        }

        private string ServerCommuniactionObj_OnReceive(object message, params object[] param)
        {
            string id = Guid.NewGuid().ToString();
            string clientObj = $"{param[0]}";
            var type = JsonHelper.GetJsonValue(message.ToString(), "MothedType");
            var productID = JsonHelper.GetJsonValue(message.ToString(), "ProductID");
            CreateModel model = new CreateModel { ID = id, ProductID = productID, ClientData = message.ToString().ToJsonFormat(), Type = (MothedType)Convert.ToInt32(type), ClientObj = clientObj };
            _aggregator.GetEvent<CreateDBCommand>().Publish(model);//收到信息第一时间保存到数据库
            string result = SendMES(message.ToString(), id);
            return result;
        }

        #endregion
        #region SendMES
        private string SendMES(string sourceData, string dataID)
        {
            MesResult mesResult = new MesResult();
            ReadWriteModel write;
            DataState state = DataState.ResultNG;
            string data = null;
            MesDataInfoTree mesData = null;
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateParseHandling = DateParseHandling.None;//设置不转换时间类型数据
                mesData = JsonConvert.DeserializeObject<MesDataInfoTree>(sourceData, settings);
                mesData.Url = ConfigPropertys.URL;
                mesData.Xmlns = ConfigPropertys.XMLNS;
                mesData.Xsi = ConfigPropertys.XSI;
                mesData.Xsd = ConfigPropertys.XSD;
                mesData.Soap = ConfigPropertys.SOAP;
                data = MesDataConvert.Convert(mesData, out string url);
                _logger.LogInformation($"转换后数据：\r\n{data}");
                if (ConfigPropertys.IsEnabledMES)
                {
                    if (ConfigPropertys.SelectMESType.ToUpper().Contains("WEBSERVICES"))
                    {
                        mesResult.Message = WebServicesHelper.Send(data, url).ToXMLFormat();
                    }
                    else if (ConfigPropertys.SelectMESType.ToUpper().Contains("WEBAPI"))
                    {
                        if (!string.IsNullOrEmpty(ConfigPropertys.TokenUrl))
                        {
                            mesResult.Message = WebApiHelper.Send(null, ConfigPropertys.TokenUrl).ToJsonFormat();
                            string tokenValue = JsonHelper.GetJsonValue(mesResult.Message, ConfigPropertys.TokenName);
                            url = mesData.Url.Replace(ConfigPropertys.TokenName.ToUpper(), tokenValue);
                        }
                        mesResult.Message = WebApiHelper.Send(data, url).ToJsonFormat();
                        mesResult.State = JsonHelper.GetJsonValue(mesResult.Message, ConfigPropertys.ResultName).ToUpper() == ConfigPropertys.ResultCheck;
                    }
                    else if (ConfigPropertys.SelectMESType.ToUpper().Contains("CLIENT"))
                    {
                        write = new ReadWriteModel(data);
                        mesResult.State = MESObj.Write(ref write, true);
                        mesResult.Message = write.Result.ToString().ToJsonFormat();
                    }
                    state = mesResult.State ? DataState.ResultOK : DataState.ResultNG;
                }
                else
                {
                    mesResult.Message = "未启用MES！！！";
                    mesResult.State = true;
                    state = DataState.UnUpLoad;
                }
            }
            catch (Exception ex)
            {
                mesResult.Message = ex.Message;
                mesResult.State = false;
                _logger.LogError($"{ex.Message}");
            }
            _aggregator.GetEvent<UpdateStateCommand>().Publish(new UpdateDBStateMessage { ID = dataID, State = state, MESResult = mesResult.Message });//收到MES结果保存到数据库
            _aggregator.GetEvent<MessageEvent>().Publish(new MessageModel { Type = mesData.MothedType.ToString(), SourceData = sourceData, ConvertedData = data, ResultData = mesResult.Message });
            return JsonConvert.SerializeObject(mesResult);
        }
        #endregion

        #endregion
        public static void UIAction(Action action)//在主线程外激活线程方法
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }
    }
}
