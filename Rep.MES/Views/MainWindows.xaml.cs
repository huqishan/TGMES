using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Regions;
using Rep.Controls.CustomContorls;
using Rep.MES.ViewModels;
using Shared.Infrastructure.PackMethod;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Rep.MES.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindows : Window
    {
        private MainWindowsViewModel _vm;
        public MainWindows()
        {
            InitializeComponent();
            _vm = DataContext as MainWindowsViewModel;
            Task.Run(() =>
                {
                    AutoResetEvent isEnd = new AutoResetEvent(false);
                    foreach (var item in DataManage)
                    {
                        isEnd.Reset();
                        AddItem(item, data, isEnd);
                        Thread.Sleep(200);
                        isEnd.WaitOne(1000);
                    }
                    foreach (var item in MESManage)
                    {
                        isEnd.Reset();
                        AddItem(item, mes, isEnd);
                        Thread.Sleep(200);
                        isEnd.WaitOne(1000);
                    }

                    UIAction(() => { _vm.ProgressBar = 100; _vm.LoadText = $"加载界面完成"; _vm.Navigate("Home"); });
                    Thread.Sleep(200);
                    UIAction(() => { 
                        Loading.Visibility = Visibility.Collapsed; 
                        main.Visibility = Visibility.Visible;
                        main.OpacityMask = this.Resources["ShowBrush"] as LinearGradientBrush;
                        Storyboard std = this.Resources["ShowStoryboard"] as Storyboard;
                        std.Begin();
                    });
                    void AddItem(KeyValuePair<string, PackIconKind> keyValuePair, TreeViewItem treeView, AutoResetEvent @event)
                    {
                        UIAction(() =>
                    {
                        _vm.LoadText = $"正在加载{keyValuePair.Key}界面";
                        treeView.Items.Add(GetItem(keyValuePair.Key, keyValuePair.Value));
                        _vm.Navigate(keyValuePair.Key.Split('-')[1].ToString());
                        _vm.ProgressBar += 5;
                        @event.Set();
                    });
                    }

                });
        }
        private void Viewbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _vm.IsShowDrawer = false;

            Task.Run(() =>
            {
                UIAction(() =>
                {
                    if (sender is TreeViewItem viewItem)
                    {
                        _vm.Navigate(viewItem.Tag.ToString());
                    }
                });
            });
        }

        private TreeViewItem GetItem(string text, PackIconKind iconPack)
        {
            TreeViewItem viewItem = new TreeViewItem();
            StackPanel panel = new StackPanel();
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Orientation = Orientation.Horizontal;
            TextBlock textBlock = new TextBlock() { Text = text.Split('-')[0] };
            textBlock.Padding = new Thickness(5, 1, 0, 0);
            PackIcon packIcon = new PackIcon();
            packIcon.Kind = iconPack;

            panel.Children.Add(packIcon);
            panel.Children.Add(textBlock);
            viewItem.Header = panel;
            viewItem.MouseLeftButtonUp += Viewbox_MouseLeftButtonUp;
            viewItem.Tag = text.Split('-')[1];
            return viewItem;
        }
        private Dictionary<string, PackIconKind> MESManage = new Dictionary<string, PackIconKind>()
        {
            {"主界面-Home",PackIconKind.Home },
            {"配置设置-Config", PackIconKind.Cog},
            {"用户登录-UserLogin",PackIconKind.AccountArrowUp },
            {"用户登出-UserLogOut",PackIconKind.AccountArrowDown },
            {"产品进站-StationIn",PackIconKind.ChevronTripleUp },
            {"产品出站-StationOut",PackIconKind.ChevronTripleDown },
            {"发送实验数据-UpLoadTestData",PackIconKind.MessageFast },
            {"设备报警-Alarm",PackIconKind.AlarmLight },
            {"设备状态改变-StateChanged",PackIconKind.StateMachine }
        };
        private Dictionary<string, PackIconKind> DataManage = new Dictionary<string, PackIconKind>()
        {
            {"白班数据-White", PackIconKind.Brightness5},
            {"夜班数据-Black", PackIconKind.Brightness4},
            {"数据查询-SelectData", PackIconKind.DatabaseSearch},
            {"通讯数据查询-CommunicationData",PackIconKind.DatabaseSearch }
        };

        public static void UIAction(Action action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
        }

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listbox)
            {
                switch (listbox.SelectedIndex)
                {
                    case 0:
                        WindowState = WindowState.Maximized;
                        break;
                    case 1:
                        this.WindowState = System.Windows.WindowState.Minimized;
                        break;
                    case 2:
                        this.WindowState = System.Windows.WindowState.Normal;
                        break;
                    case 3:
                        //this.IsEnabled = false;
                        FadeOut(() => this.Close());
                        break;
                    default:
                        break;
                }
            }

        }
        private void FadeOut(Action action)
        {
            main.OpacityMask = this.Resources["ClosedBrush"] as LinearGradientBrush;
            Storyboard std = this.Resources["ClosedStoryboard"] as Storyboard;
            std.Completed += delegate { action.Invoke(); };
            std.Begin();
        }
    }
}
