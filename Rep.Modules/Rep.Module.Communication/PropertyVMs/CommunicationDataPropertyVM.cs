using Prism.Mvvm;
using Rep.Controls.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rep.Module.Communication.PropertyVMs
{
    public class CommunicationDataPropertyVM : BindableBase
    {
        #region Date
        private DateTime _Date = DateTime.Now;

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (value != _Date)
                {
                    SetProperty(ref _Date, value);
                }
            }
        }
        #endregion
        #region ClassesList
        private ObservableCollection<string> _ClassesList = new ObservableCollection<string>() { "全部", "白班", "夜班" };

        public ObservableCollection<string> ClassesList
        {
            get { return _ClassesList; }
            set
            {
                if (value != _ClassesList)
                {
                    SetProperty(ref _ClassesList, value);
                }
            }
        }
        #endregion 
        #region Classes
        private string _Classes = "全部";

        public string Classes
        {
            get { return _Classes; }
            set
            {
                if (value != _Classes)
                {
                    SetProperty(ref _Classes, value);
                }
            }
        }
        #endregion
        #region ResultList
        private ObservableCollection<string> _ResultList = new ObservableCollection<string>() { "全部", "OK", "NG" };

        public ObservableCollection<string> ResultList
        {
            get { return _ResultList; }
            set
            {
                if (value != _ResultList)
                {
                    SetProperty(ref _ResultList, value);
                }
            }
        }
        #endregion 
        #region Result
        private string _Result = "全部";

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
        #region MothedTypeList
        private ObservableCollection<string> _MothedTypeList = new ObservableCollection<string> { "全部", "报警", "用户登录", "用户登出", "进站", "出站", "测试数据", "状态改变" };
        public ObservableCollection<string> MothedTypeList
        {
            get { return _MothedTypeList; }
            set
            {
                if (value != _MothedTypeList)
                {
                    SetProperty(ref _MothedTypeList, value);
                }
            }
        }
        #endregion
        #region SelectedMothedType
        private int _SelectedMothedType = 0;

        public int SelectedMothedType
        {
            get { return _SelectedMothedType; }
            set
            {
                if (value != _SelectedMothedType)
                {
                    SetProperty(ref _SelectedMothedType, value);
                }
            }
        }
        #endregion 
        #region StartDate
        private DateTime _StartDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                if (value != _StartDate)
                {
                    SetProperty(ref _StartDate, value);
                }
            }
        }
        #endregion
        #region StartTime
        private DateTime _StartTime = DateTime.Now;

        public DateTime StartTime
        {
            get { return _StartTime; }
            set
            {
                if (value != _StartTime)
                {
                    SetProperty(ref _StartTime, value);
                }
            }
        }
        #endregion
        #region EndDate
        private DateTime _EndDate = DateTime.Now;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                if (value != _EndDate)
                {
                    SetProperty(ref _EndDate, value);
                }
            }
        }
        #endregion
        #region EndTime
        private DateTime _EndTime = DateTime.Now;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                if (value != _EndTime)
                {
                    SetProperty(ref _EndTime, value);
                }
            }
        }
        #endregion
        #region ProductID
        private string _ProductID;

        public string ProductID
        {
            get { return _ProductID; }
            set
            {
                if (value != _ProductID)
                {
                    SetProperty(ref _ProductID, value);
                }
            }
        }
        #endregion
        #region IsTimeSelect
        private bool _IsTimeSelect;

        public bool IsTimeSelect
        {
            get { return _IsTimeSelect; }
            set
            {
                if (value != _IsTimeSelect)
                {
                    SetProperty(ref _IsTimeSelect, value);
                }
            }
        }
        #endregion
        #region ExportAnimation
        private bool _ExportAnimation;

        public bool ExportAnimation
        {
            get { return _ExportAnimation; }
            set
            {
                if (value != _ExportAnimation)
                {
                    SetProperty(ref _ExportAnimation, value);
                }
            }
        }
        #endregion
        #region ManualAnimation
        private bool _ManualAnimation;

        public bool ManualAnimation
        {
            get { return _ManualAnimation; }
            set
            {
                if (value != _ManualAnimation)
                {
                    SetProperty(ref _ManualAnimation, value);
                }
            }
        }
        #endregion 
        #region Count
        private string _Count = "总数 0";

        public string Count
        {
            get { return _Count; }
            set
            {
                if (value != _Count)
                {
                    SetProperty(ref _Count, value);
                }
            }
        }
        #endregion
        #region OKCount
        private string _OKCount = "OK总数 0 (0%)";

        public string OKCount
        {
            get { return _OKCount; }
            set
            {
                if (value != _OKCount)
                {
                    SetProperty(ref _OKCount, value);
                }
            }
        }
        #endregion
        #region NGCount
        private string _NGCount = "NG总数 0 (0%)";

        public string NGCount
        {
            get { return _NGCount; }
            set
            {
                if (value != _NGCount)
                {
                    SetProperty(ref _NGCount, value);
                }
            }
        }
        #endregion 
        #region IsAllSelected
        private bool _IsAllSelected;

        public bool IsAllSelected
        {
            get { return _IsAllSelected; }
            set
            {
                if (value != _IsAllSelected)
                {
                    SetProperty(ref _IsAllSelected, value);
                    foreach (var item in CommunicationData)
                    {
                        item.IsSelected = value;
                    }
                }
            }
        }
        #endregion 
        #region CommunicationData
        private List<CommunicationDataModel> _CommunicationData = new List<CommunicationDataModel>();
        //=new List<CommunicationDataModel> {
        //new CommunicationDataModel{ Id="122", ClientData="fds", MothedType=0, ClientObj="127.0.0.1:3333", MESResult="f", RecordTime=DateTime.Now, State=0 }
        //};

        public List<CommunicationDataModel> CommunicationData
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
        #region Loading
        private bool _Loading=false;

        public bool Loading
        {
            get { return _Loading; }
            set
            {
                if (value != _Loading)
                {
                    SetProperty(ref _Loading, value);
                }
            }
        }
        #endregion 
    }
}
