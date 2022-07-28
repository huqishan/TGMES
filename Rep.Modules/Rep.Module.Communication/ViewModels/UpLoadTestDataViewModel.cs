using Prism.Events;
using Prism.Mvvm;
using Rep.Controls.CustomContorls;
using Rep.Controls.Events;
using Rep.Controls.Model;

namespace Rep.Module.Communication.ViewModels
{
    internal class UpLoadTestDataViewModel : BindableBase
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
        public UpLoadTestDataViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.GetEvent<MessageEvent>().Subscribe(MessageHandle, arg => arg.Type.ToUpper() == "TESTDATA");
        }
        private void MessageHandle(MessageModel model)
        {
            _DataVM.MessageHadle(model);
        }
    }
}
