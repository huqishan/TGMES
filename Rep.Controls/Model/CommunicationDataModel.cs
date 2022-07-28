using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.Model
{
    public class CommunicationDataModel : BindableBase
    {
        public string Id { get; set; }
        public string ProductID { get; set; }
        public string ClientData { get; set; }
        public string MESResult { get; set; }
        public string RecordTime { get; set; }
        public int State { get; set; }
        public int MothedType { get; set; }
        public string ClientObj { get; set; }
        #region IsSelected
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    SetProperty(ref _IsSelected, value);
                }
            }
        }
        #endregion 
    }
}
