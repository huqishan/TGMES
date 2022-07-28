using Prism.Events;
using Prism.Mvvm;
using Rep.Controls.CustomContorls;
using Rep.Controls.Events;
using Rep.Controls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.MES.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region DataVM
        private DataShowControl _DataVM=new DataShowControl();

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
        IEventAggregator _aggregator;
        public MainViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            aggregator.GetEvent<MessageEvent>().Subscribe(MessageHandle);
        }
        private void MessageHandle(MessageModel model)
        {
            _DataVM.MessageHadle(model);
        }
    }
}
