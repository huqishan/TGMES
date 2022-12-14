using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Module.Data.ViewModels
{
    internal class SelectDataViewModel : BindableBase
    {
        #region Propertys
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
        private string _Classes="全部";

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
        #region IsShow
        private bool _IsShow;

        public bool IsShow
        {
            get { return _IsShow; }
            set
            {
                if (value != _IsShow)
                {
                    SetProperty(ref _IsShow, value);
                }
            }
        }
        #endregion 
        IDialogService _dialogService;
        #endregion
        public SelectDataViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        #region Method
        private DelegateCommand _SelectCommand;
        public DelegateCommand SelectCommand => _SelectCommand ??= new DelegateCommand(SelectExecute);
        private void SelectExecute()
        {
            _dialogService.ShowDialog("MessageDialogView");
        }
        #endregion
    }
}
