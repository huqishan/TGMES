using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Rep.Controls.CustomContorls.PropertyVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.CustomContorls
{
    public class AddItemDialogControl : BindableBase, IDialogAware
    {
        #region Property
        private AddItemDialogPropertyVM _Property=new AddItemDialogPropertyVM();

        public AddItemDialogPropertyVM Property
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
        private DelegateCommand _getMessageCommand;
        private DelegateCommand _cancelMessageCommand;
        public DelegateCommand GetMessageCommand
        {
            get => _getMessageCommand = new DelegateCommand(() =>
            {
                var parameter = new DialogParameters();
                parameter.Add("ClientCode", $"{Property.ClientCode}");
                parameter.Add("MESCode", $"{Property.MESCode}");
                parameter.Add("DataType", $"{Property.SelectedDataType}");
                parameter.Add("KeepDecimalLength", $"{Property.KeepDecimalLength}");
                parameter.Add("DefectValue", $"{Property.DefectValue}");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameter));
            });
        }

        public DelegateCommand CancelMessageCommand
        {
            get => _cancelMessageCommand = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
        }

        public string Title => "Message";
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// 允许用户手动关闭当前窗口
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 关闭dialog的操作
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDialogClosed()
        {

        }

        /// <summary>
        /// dialog接收参数传递
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            var parameterContent = parameters.GetValue<string>("Value");

        }
    }
}
