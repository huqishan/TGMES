using Newtonsoft.Json.Linq;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Rep.MES.ViewModels
{
    public class MainWindowsViewModel : BindableBase
    {
        #region Propertys
        #region IsShowDrawer
        private bool _IsShowDrawer;

        public bool IsShowDrawer
        {
            get { return _IsShowDrawer; }
            set
            {
                if (value != _IsShowDrawer)
                {
                    SetProperty(ref _IsShowDrawer, value);
                }
            }
        }
        #endregion
        #region LoadText
        private string _LoadText;

        public string LoadText
        {
            get { return _LoadText; }
            set
            {
                if (value != _LoadText)
                {
                    SetProperty(ref _LoadText, value);
                }
            }
        }
        #endregion
        #region ProgressBar
        private int _ProgressBar = 0;

        public int ProgressBar
        {
            get { return _ProgressBar; }
            set
            {
                if (value != _ProgressBar)
                {
                    SetProperty(ref _ProgressBar, value);
                }
            }
        }
        #endregion
        #region Version
        private string _Version="版本号：1.0.0.0";

        public string Version
        {
            get { return _Version; }
            set
            {
                if (value != _Version)
                {
                    SetProperty(ref _Version, value);
                }
            }
        }
        #endregion 
        #endregion

        IRegionManager _regionManager;
        public MainWindowsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string verDate = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString("yyMMddHH").Substring(0, 8);
            Version = $"V {ver} {verDate}"; //版本
        }
        public void Navigate(string view)
        {
            _regionManager.RequestNavigate("ContentRegion", $"{view}View");
        }

    }
}
