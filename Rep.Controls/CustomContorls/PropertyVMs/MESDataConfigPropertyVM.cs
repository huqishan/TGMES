using Prism.Mvvm;
using Rep.Controls.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.CustomContorls.PropertyVMs
{
    public class MESDataConfigPropertyVM : BindableBase
    {
        #region Items
        private ObservableCollection<TreeModel> _Items = new ObservableCollection<TreeModel>() { new TreeModel() {
                ClientCode="Add Mothed",
                IsRoot=true,
            }
        };

        public ObservableCollection<TreeModel> Items
        {
            get { return _Items; }
            set
            {
                if (value != _Items)
                {
                    SetProperty(ref _Items, value);
                }
            }
        }
        #endregion
        #region SelectedItem
        private TreeModel _SelectedItem;

        public TreeModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (value != _SelectedItem)
                {
                    SetProperty(ref _SelectedItem, value);
                }
            }
        }
        #endregion
    }
}
