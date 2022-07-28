using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.CustomContorls.PropertyVMs
{
    public class AddItemDialogPropertyVM: BindableBase
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
        #region KeepDecimalLength
        private int _KeepDecimalLength;

        public int KeepDecimalLength
        {
            get { return _KeepDecimalLength; }
            set
            {
                if (value != _KeepDecimalLength)
                {
                    SetProperty(ref _KeepDecimalLength, value);
                }
            }
        }
        #endregion
        #region DataTypes
        
        private ObservableCollection<string> _DataTypes=new ObservableCollection<string>() {"String", "StringArr", "Int", "IntArr", "Double","DoubleArr" };

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
        private string _SelectedDataType="String";

        public string SelectedDataType
        {
            get { return _SelectedDataType; }
            set
            {
                if (value != _SelectedDataType)
                {
                    SetProperty(ref _SelectedDataType, value);
                    IsEnabledDecimal = value.Contains("Double");
                }
            }
        }
        #endregion
        #region IsEnabledDecimal
        private bool _IsEnabledDecimal;

        public bool IsEnabledDecimal
        {
            get { return _IsEnabledDecimal; }
            set
            {
                if (value != _IsEnabledDecimal)
                {
                    SetProperty(ref _IsEnabledDecimal, value);
                }
            }
        }
        #endregion
        #region DefectValue
        private string _DefectValue;

        public string DefectValue
        {
            get { return _DefectValue; }
            set
            {
                if (value != _DefectValue)
                {
                    SetProperty(ref _DefectValue, value);
                }
            }
        }
        #endregion 
        #endregion
    }
}
