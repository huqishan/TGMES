using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.Model
{
    public class TreeModel : BindableBase
    {
        #region IsRoot
        private bool _IsRoot;

        public bool IsRoot
        {
            get { return _IsRoot; }
            set
            {
                if (value != _IsRoot)
                {
                    SetProperty(ref _IsRoot, value);
                }
            }
        }
        #endregion 
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
        #region DataType
        private string _DataType;

        public string DataType
        {
            get { return _DataType; }
            set
            {
                if (value != _DataType)
                {
                    SetProperty(ref _DataType, value);
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
        #region KeepDecimalLength
        private string _KeepDecimalLength;

        public string KeepDecimalLength
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
        #region Childs
        private ObservableCollection<TreeModel> _Childs = new ObservableCollection<TreeModel>();

        public ObservableCollection<TreeModel> Childs
        {
            get { return _Childs; }
            set
            {
                if (value != _Childs)
                {
                    SetProperty(ref _Childs, value);
                }
            }
        }
        #endregion 
    }
}
