using Prism.Events;
using Prism.Mvvm;
using Rep.Controls.CustomContorls;
using Rep.Controls.Events;
using Rep.Controls.Model;
using System;

namespace Rep.Module.Communication.ViewModels
{
    internal class StationInViewModel : BindableBase
    {
        #region Propertys

        #region DataVM
        private DataShowControl _DataVM = new();

        public DataShowControl DataVM
        {
            get { return _DataVM; }
            set
            {
                if (value != _DataVM)
                {
                    SetProperty(ref _DataVM, value);
                }
            }
        }
        #endregion

        #endregion
        IEventAggregator _aggregator;
        public StationInViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.GetEvent<MessageEvent>().Subscribe(MessageHandle,arg=>arg.Type.ToUpper()=="STATIONIN");
        }
        private void MessageHandle(MessageModel model)
        {
            _DataVM.MessageHadle(model);
        }
        public static void UIAction(Action action)//在主线程外激活线程方法
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }
    }
}
