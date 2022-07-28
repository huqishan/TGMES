using Newtonsoft.Json;
using Prism.Mvvm;
using Rep.Controls.Converts;
using Rep.Controls.Model;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.PackMethod;
using System;
using System.Collections.ObjectModel;

namespace Rep.Controls.CustomContorls
{
    public class DataShowControl : BindableBase
    {
        #region Propertys
        #region SourceData
        private ObservableCollection<string> _SourceData = new ObservableCollection<string>();

        public ObservableCollection<string> SourceData
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
        private ObservableCollection<string> _ConvertedData = new ObservableCollection<string>();

        public ObservableCollection<string> ConvertedData
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
        private ObservableCollection<string> _Result = new ObservableCollection<string>();

        public ObservableCollection<string> Result
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
        #region Method
        public void MessageHadle(MessageModel model)
        {
            if (SourceData.Count > 10)
            {
                SourceData.RemoveAt(10);
                ConvertedData.RemoveAt(10);
                Result.RemoveAt(10);
            }

            UIAction(() => {
                SourceData.Insert(0, model.SourceData.ToJsonFormat());
                ConvertedData.Insert(0, model.ConvertedData);
                Result.Insert(0, model.ResultData);
            });
        }
        public static void UIAction(Action action)//在主线程外激活线程方法
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }
        #endregion
    }
}
