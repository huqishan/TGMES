using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.CustomContorls
{
    public class TextBoxDialogControl : BindableBase, IDialogAware
    {
        #region Propertys
        #region ClientCode
        private string _ClientCode;

        public string ClientCode
        {
            get { return _ClientCode; }
            set
            {
                if (value != _ClientCode)
                {
                    SetProperty(ref _ClientCode, value);
                }
            }
        }
        #endregion
        #region MESCode
        private string _MESCode;

        public string MESCode
        {
            get { return _MESCode; }
            set
            {
                if (value != _MESCode)
                {
                    SetProperty(ref _MESCode, value);
                }
            }
        }
        #endregion
        #region DataTypes
        private ObservableCollection<string> _DataTypes=new ObservableCollection<string>() {"JSON","XML","SOAP" };

        public ObservableCollection<string> DataTypes
        {
            get { return _DataTypes; }
            set
            {
                if (value != _DataTypes)
                {
                    SetProperty(ref _DataTypes, value);
                }
            }
        }
        #endregion
        #region SelectedDataType
        private string _SelectedDataType="SOAP";

        public string SelectedDataType
        {
            get { return _SelectedDataType; }
            set
            {
                if (value != _SelectedDataType)
                {
                    SetProperty(ref _SelectedDataType, value);
                }
            }
        }
        #endregion 
        #endregion
        #region Mothed
        private DelegateCommand _getMessageCommand;
        private DelegateCommand _cancelMessageCommand;
        public DelegateCommand GetMessageCommand
        {
            get => _getMessageCommand = new DelegateCommand(() =>
            {
                var parameter = new DialogParameters();
                parameter.Add("ClientCode", $"{ClientCode}");
                parameter.Add("MESCode", $"{MESCode}");
                parameter.Add("DataType", $"{SelectedDataType}");
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
        #endregion
    }
}
