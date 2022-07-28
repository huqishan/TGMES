using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Rep.Controls.CustomContorls.PropertyVMs;
using Rep.Controls.Model;
using Shared.Infrastructure.PackMethod;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.CustomContorls
{
    public class MESDataConfigControl : BindableBase
    {
        #region Propertys
        #region Property
        private MESDataConfigPropertyVM _Property = new MESDataConfigPropertyVM();

        public MESDataConfigPropertyVM Property
        {
            get { return _Property; }
            set
            {
                if (value != _Property)
                {
                    SetProperty(ref _Property, value);
                }
            }
        }
        #endregion 
        IDialogService _dialogService;
        readonly string file = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MESConfig";
        #endregion

        public MESDataConfigControl(IDialogService dialogService)
        {
            _dialogService = dialogService;
            LoadJSON();
        }
        #region Method
        #region Add
        private DelegateCommand _AddCommand;
        public DelegateCommand AddCommand => _AddCommand ??= new DelegateCommand(AddExecute);
        private void AddExecute()
        {
            if (Property.SelectedItem == null || Property.SelectedItem.ClientCode == "Add Mothed")
            {
                _dialogService.ShowDialog("TextBoxDialogView", (r) =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        var clientCode = r.Parameters.GetValue<string>("ClientCode");
                        var mesCode = r.Parameters.GetValue<string>("MESCode");
                        var dataType = r.Parameters.GetValue<string>("DataType");
                        Property.Items.Add(new TreeModel()
                        {
                            ClientCode = clientCode,
                            MESCode = mesCode,
                            DataType = dataType,
                            IsRoot = true,
                        });
                    }
                });
            }
            else
            {
                _dialogService.ShowDialog("AddItemDialogView", (r) =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        var clientCode = r.Parameters.GetValue<string>("ClientCode");
                        var mesCode = r.Parameters.GetValue<string>("MESCode");
                        var dataType = r.Parameters.GetValue<string>("DataType");
                        var keepDecimalLength = r.Parameters.GetValue<int>("KeepDecimalLength");
                        var defectValue = r.Parameters.GetValue<string>("DefectValue");
                        Property.SelectedItem.Childs.Add(new TreeModel
                        {
                            ClientCode = clientCode,
                            MESCode = mesCode,
                            DataType = dataType,
                            DefectValue = defectValue,
                            KeepDecimalLength = keepDecimalLength == 0 ? "" : keepDecimalLength.ToString(),
                        });
                    }
                });

            }
        }
        #endregion
        #region Up
        private DelegateCommand _UpCommand;
        public DelegateCommand UpCommand => _UpCommand ??= new DelegateCommand(UpExecute);
        private void UpExecute()
        {
            var parent = GetParentModel(Property.SelectedItem, Property.Items);
            int index = parent.Childs.IndexOf(Property.SelectedItem);
            TreeModel cache;
            if (index > 0)
            {
                cache = parent.Childs[index - 1];
                parent.Childs[index - 1] = Property.SelectedItem;
                parent.Childs[index] = cache;
            }
        }
        #endregion
        #region Down
        private DelegateCommand _DownCommand;
        public DelegateCommand DownCommand => _DownCommand ??= new DelegateCommand(DownExecute);
        private void DownExecute()
        {
            var parent = GetParentModel(Property.SelectedItem, Property.Items);
            int index = parent.Childs.IndexOf(Property.SelectedItem);
            TreeModel cache;
            if (index < parent.Childs.Count - 1)
            {
                cache = parent.Childs[index + 1];
                parent.Childs[index + 1] = Property.SelectedItem;
                parent.Childs[index] = cache;
            }
        }
        #endregion
        #region Remove
        private DelegateCommand _RemoveCommand;
        public DelegateCommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(RemoveExecute);
        private void RemoveExecute()
        {
            var parent = GetParentModel(Property.SelectedItem, Property.Items);
            if (parent != null)
                parent.Childs.Remove(Property.SelectedItem);
            else
                Property.Items.Remove(Property.SelectedItem);
            Property.SelectedItem = null;
        }
        #endregion
        #region Selected
        private DelegateCommand<object> _SelectedCommand;
        public DelegateCommand<object> SelectedCommand => _SelectedCommand ??= new DelegateCommand<object>(SelectedExecute);
        private void SelectedExecute(object obj)
        {
            if (obj is TreeModel model)
            {
                Property.SelectedItem = model;
            }
        }
        #endregion
        #region MouseDoubleClick
        private DelegateCommand<object> _MouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand => _MouseDoubleClickCommand ??= new DelegateCommand<object>(MouseDoubleClickExecute);
        private void MouseDoubleClickExecute(object obj)
        {
            if (obj is TreeModel model)
            {
                AddExecute();
            }
        }
        #endregion
        #region Save
        private DelegateCommand<object> _SaveCommand;
        public DelegateCommand<object> SaveCommand => _SaveCommand ??= new DelegateCommand<object>(SaveExecute);
        private void SaveExecute(object obj)
        {
            foreach (var item in Property.Items)
            {
                if (item.ClientCode != "Add Mothed")
                    JsonHelper.SaveJson(item, $"{file}\\{item.ClientCode}.json");
            }
        }
        #endregion
        private void LoadJSON()
        {
            if (Directory.Exists(file))
                foreach (var item in Directory.GetFiles(file))
                {
                    Property.Items.Add(JsonHelper.ReadJson<TreeModel>(item));
                }
        }
        private TreeModel? GetParentModel(TreeModel model, IEnumerable<TreeModel> parent)
        {
            TreeModel? parentModel = null;
            foreach (var item in parent)
            {
                parentModel = item.Childs.FirstOrDefault(x => x == model);
                if (parentModel == null)
                {
                    parentModel = GetParentModel(model, item.Childs);
                    if (parentModel != null)
                        break;
                }
                else
                {
                    parentModel = item;
                    return parentModel;
                }
            }
            return parentModel;
        }
        #endregion
    }
}
