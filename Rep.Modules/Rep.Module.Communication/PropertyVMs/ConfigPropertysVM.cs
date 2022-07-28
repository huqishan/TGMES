using Newtonsoft.Json;
using Prism.Mvvm;
using Rep.Controls.CustomContorls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Module.Communication.PropertyVMs
{
    public class ConfigPropertysVM : BindableBase
    {
        #region Propertys
        #region 客户端通讯配置
        #region RabbitMQAddress
        private string _RabbitMQAddress="127.0.0.1";

        public string RabbitMQAddress
        {
            get { return _RabbitMQAddress; }
            set
            {
                if (value != _RabbitMQAddress)
                {
                    SetProperty(ref _RabbitMQAddress, value);
                }
            }
        }
        #endregion
        #region RabbitMQPort
        private ushort _RabbitMQPort=5672;

        public ushort RabbitMQPort
        {
            get { return _RabbitMQPort; }
            set
            {
                if (value != _RabbitMQPort)
                {
                    SetProperty(ref _RabbitMQPort, value);
                }
            }
        }
        #endregion
        #region RabbitMQUserName
        private string _RabbitMQUserName="guest";

        public string RabbitMQUserName
        {
            get { return _RabbitMQUserName; }
            set
            {
                if (value != _RabbitMQUserName)
                {
                    SetProperty(ref _RabbitMQUserName, value);
                }
            }
        }
        #endregion
        #region RabbitMQPassWord
        private string _RabbitMQPassWord="guest";

        public string RabbitMQPassWord
        {
            get { return _RabbitMQPassWord; }
            set
            {
                if (value != _RabbitMQPassWord)
                {
                    SetProperty(ref _RabbitMQPassWord, value);
                }
            }
        }
        #endregion 
        #region CommunicationData
        private ObservableCollection<string> _CommunicationData = new ObservableCollection<string>();
        [JsonIgnore]
        public ObservableCollection<string> CommunicationData
        {
            get { return _CommunicationData; }
            set
            {
                if (value != _CommunicationData)
                {
                    SetProperty(ref _CommunicationData, value);
                }
            }
        }
        #endregion
        #endregion
        #region MES通讯配置
        #region IsEnabledMES
        private bool _IsEnabledMES = true;

        public bool IsEnabledMES
        {
            get { return _IsEnabledMES; }
            set
            {
                if (value != _IsEnabledMES)
                {
                    SetProperty(ref _IsEnabledMES, value);
                }
            }
        }
        #endregion 
        #region MESTypes
        private ObservableCollection<string> _MESTypes = new ObservableCollection<string> { "TCP Client", "WebApi", "WebServices" };
        [JsonIgnore]
        public ObservableCollection<string> MESTypes
        {
            get { return _MESTypes; }
            set
            {
                if (value != _MESTypes)
                {
                    SetProperty(ref _MESTypes, value);
                }
            }
        }
        #endregion 
        #region SelectMESType
        private string _SelectMESType = "WebServices";

        public string SelectMESType
        {
            get { return _SelectMESType; }
            set
            {
                if (value != _SelectMESType)
                {
                    SetProperty(ref _SelectMESType, value);
                }
            }
        }
        #endregion
        #region MESLocalIpAddress
        private string _MESLocalIpAddress;

        public string MESLocalIpAddress
        {
            get { return _MESLocalIpAddress; }
            set
            {
                if (value != _MESLocalIpAddress)
                {
                    SetProperty(ref _MESLocalIpAddress, value);
                }
            }
        }
        #endregion
        #region MESLoaclPort
        private ushort _MESLoaclPort;

        public ushort MESLoaclPort
        {
            get { return _MESLoaclPort; }
            set
            {
                if (value != _MESLoaclPort)
                {
                    SetProperty(ref _MESLoaclPort, value);
                }
            }
        }
        #endregion
        #region MESRemoteIpAddress
        private string _MESRemoteIpAddress;

        public string MESRemoteIpAddress
        {
            get { return _MESRemoteIpAddress; }
            set
            {
                if (value != _MESRemoteIpAddress)
                {
                    SetProperty(ref _MESRemoteIpAddress, value);
                }
            }
        }
        #endregion
        #region MESRemotePort
        private ushort _MESRemotePort;

        public ushort MESRemotePort
        {
            get { return _MESRemotePort; }
            set
            {
                if (value != _MESRemotePort)
                {
                    SetProperty(ref _MESRemotePort, value);
                }
            }
        }
        #endregion
        #region MESCommunicationData
        private ObservableCollection<string> _MESCommunicationData = new ObservableCollection<string>();
        [JsonIgnore]
        public ObservableCollection<string> MESCommunicationData
        {
            get { return _MESCommunicationData; }
            set
            {
                if (value != _MESCommunicationData)
                {
                    SetProperty(ref _MESCommunicationData, value);
                }
            }
        }
        #endregion
        #region URL
        private string _URL;
        public string URL
        {
            get { return _URL; }
            set
            {
                if (value != _URL)
                {
                    SetProperty(ref _URL, value);
                }
            }
        }
        #endregion
        #region XSI
        private string _XSI;

        public string XSI
        {
            get { return _XSI; }
            set
            {
                if (value != _XSI)
                {
                    SetProperty(ref _XSI, value);
                }
            }
        }
        #endregion
        #region XSD
        private string _XSD;

        public string XSD
        {
            get { return _XSD; }
            set
            {
                if (value != _XSD)
                {
                    SetProperty(ref _XSD, value);
                }
            }
        }
        #endregion
        #region SOAP
        private string _SOAP;

        public string SOAP
        {
            get { return _SOAP; }
            set
            {
                if (value != _SOAP)
                {
                    SetProperty(ref _SOAP, value);
                }
            }
        }
        #endregion
        #region XMLNS
        private string _XMLNS;

        public string XMLNS
        {
            get { return _XMLNS; }
            set
            {
                if (value != _XMLNS)
                {
                    SetProperty(ref _XMLNS, value);
                }
            }
        }
        #endregion
        #region TokenUrl
        private string _TokenUrl;

        public string TokenUrl
        {
            get { return _TokenUrl; }
            set
            {
                if (value != _TokenUrl)
                {
                    SetProperty(ref _TokenUrl, value);
                }
            }
        }
        #endregion
        #region TokenName
        private string _TokenName= "accessToken";

        public string TokenName
        {
            get { return _TokenName; }
            set
            {
                if (value != _TokenName)
                {
                    SetProperty(ref _TokenName, value);
                }
            }
        }
        #endregion
        #region ResultName
        private string _ResultName= "msg";

        public string ResultName
        {
            get { return _ResultName; }
            set
            {
                if (value != _ResultName)
                {
                    SetProperty(ref _ResultName, value);
                }
            }
        }
        #endregion
        #region ResultCheck
        private string _ResultCheck="成功";

        public string ResultCheck
        {
            get { return _ResultCheck; }
            set
            {
                if (value != _ResultCheck)
                {
                    SetProperty(ref _ResultCheck, value);
                }
            }
        }
        #endregion 
        #endregion
        #region TestMES
        #region DataTypes
        private ObservableCollection<string> _DataTypes = new ObservableCollection<string> { "JSON", "XML", "SOAP" };
        [JsonIgnore]
        public ObservableCollection<string> DataTypes
        {
            get { return _DataTypes; }
            set
            {
                if (value != _DataTypes)
                {
                    SetProperty(ref _DataTypes, value);
                }
            }
        }
        #endregion 
        #region SelectedDataType
        private string _SelectedDataType;

        public string SelectedDataType
        {
            get { return _SelectedDataType; }
            set
            {
                if (value != _SelectedDataType)
                {
                    SetProperty(ref _SelectedDataType, value);
                }
            }
        }
        #endregion 
        #region MethodName
        private string _MethodName;

        public string MethodName
        {
            get { return _MethodName; }
            set
            {
                if (value != _MethodName)
                {
                    SetProperty(ref _MethodName, value);
                }
            }
        }
        #endregion
        #region SourceData
        private string _SourceData;
        [JsonIgnore]
        public string SourceData
        {
            get { return _SourceData; }
            set
            {
                if (value != _SourceData)
                {
                    SetProperty(ref _SourceData, value);
                }
            }
        }
        #endregion
        #region ConvertedData
        private string _ConvertedData;
        [JsonIgnore]
        public string ConvertedData
        {
            get { return _ConvertedData; }
            set
            {
                if (value != _ConvertedData)
                {
                    SetProperty(ref _ConvertedData, value);
                }
            }
        }
        #endregion
        #region Result
        private string _Result;
        [JsonIgnore]
        public string Result
        {
            get { return _Result; }
            set
            {
                if (value != _Result)
                {
                    SetProperty(ref _Result, value);
                }
            }
        }
        #endregion



        #endregion
        #endregion
        public ConfigPropertysVM()
        {
#if DEBUG
            _URL = "http://www.webxml.com.cn/WebServices/WeatherWebService.asmx";
            _XSI = "http://www.w3.org/2001/XMLSchema-instance";
            _XSD = "http://www.w3.org/2001/XMLSchema";
            _SOAP = "http://schemas.xmlsoap.org/soap/envelope/";
            _XMLNS = "http://WebXml.com.cn/";
#endif
        }
    }
}
