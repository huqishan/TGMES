using Prism.Events;
using Prism.Mvvm;
using Rep.Controls.CustomContorls;
using Rep.Controls.Events;
using Rep.Controls.Model;

namespace Rep.Module.Communication.ViewModels
{
    internal class UserLoginViewModel : BindableBase
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
        static IEventAggregator _aggregator;
        public UserLoginViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.GetEvent<MessageEvent>().Subscribe(MessageHandle,arg=>arg.Type.ToUpper()=="USERLOGIN");
        }
        private void MessageHandle(MessageModel model)
        {
            _DataVM.MessageHadle(model);
        }
    }
}
