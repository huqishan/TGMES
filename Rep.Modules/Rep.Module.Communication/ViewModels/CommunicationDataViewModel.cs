using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Rep.Controls.Enum;
using Rep.Controls.Events;
using Rep.Controls.Model;
using Rep.Controls.View;
using Rep.Module.Communication.PropertyVMs;
using Rep.Module.Communication.Services.CommunicationAggregate;
using Rep.Module.Communication.Services.Contexts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rep.Module.Communication.ViewModels
{
    public class CommunicationDataViewModel : BindableBase
    {
        #region CommunicationDataProperty
        private CommunicationDataPropertyVM _CommunicationDataProperty = new CommunicationDataPropertyVM();

        public CommunicationDataPropertyVM CommunicationDataProperty
        {
            get { return _CommunicationDataProperty; }
            set
            {
                if (value != _CommunicationDataProperty)
                {
                    SetProperty(ref _CommunicationDataProperty, value);
                }
            }
        }
        #endregion 
        CommuniactionContext _communiactionContext;
        IEventAggregator _aggregator;
        IDialogService _dialogService;
        public CommunicationDataViewModel(CommuniactionContext communiactionContext, IEventAggregator aggregator, IDialogService dialogService)
        {
            _communiactionContext = communiactionContext;
            _aggregator = aggregator;
            _dialogService = dialogService;
            _aggregator.GetEvent<UpLoadEvent>().Subscribe(UpLoadHandler, r => r.IsResult);
        }

        #region 方法
        private void UpLoadHandler(UpLoadMessageModel model)
        {
            CommunicationDataProperty.ManualAnimation = false;
            UIAction(() =>
            {
                DialogParameters par = new DialogParameters();
                par.Add("Message", "手动上传完成!!!");
                _dialogService.ShowDialog("MessageDialogView", par, null);

            });

        }
        #region Select
        private DelegateCommand _SelectCommand;
        public DelegateCommand SelectCommand => _SelectCommand ??= new DelegateCommand(SelectExecute);
        private void SelectExecute()
        {
            CommunicationDataProperty.Loading = true;
            CommunicationDataProperty.CommunicationData?.Clear();
            CommunicationDataProperty.IsAllSelected = false;
            Task.Run(() =>
              {
                  string sql = null;
                  string startTime = null;
                  string endTime = null;
                  if (CommunicationDataProperty.Classes.Contains("白班"))
                  {
                      startTime = CommunicationDataProperty.StartDate.Date.AddHours(8).AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss");
                      endTime = CommunicationDataProperty.Date.Date.AddHours(20).AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss");
                  }
                  else if (CommunicationDataProperty.Classes.Contains("夜班"))
                  {
                      startTime = CommunicationDataProperty.Date.Date.AddHours(20).AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss");
                      endTime = CommunicationDataProperty.Date.Date.AddDays(1).AddHours(8).AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss");
                  }
                  else
                  {
                      startTime = CommunicationDataProperty.Date.Date.ToString("yyyy-MM-dd HH:mm:ss");
                      endTime = CommunicationDataProperty.Date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss");
                  }
                  if (CommunicationDataProperty.IsTimeSelect)
                  {
                      startTime = $"{CommunicationDataProperty.StartDate:yyyy-MM-dd} {CommunicationDataProperty.StartTime:HH:mm:ss}";
                      endTime = $"{CommunicationDataProperty.EndDate:yyyy-MM-dd} {CommunicationDataProperty.EndTime:HH:mm:ss}";
                  }
                  if (!CommunicationDataProperty.Result.Contains("全部"))
                  {
                      sql = $" AND State {(CommunicationDataProperty.Result.Contains("OK") ? "=" : ">")} {(int)DataState.ResultOK}";
                  }
                  if (CommunicationDataProperty.SelectedMothedType != 0)
                  {
                      sql = $" AND MothedType = {CommunicationDataProperty.SelectedMothedType - 1}";
                  }
                  sql = $" SELECT * FROM Communication WHERE RecordTime BETWEEN '{startTime}' AND '{endTime}' {sql}";
                  //$"(SELECT ROW_NUMBER() OVER(ORDER BY RecordTime ASC) AS rowid,* FROM Communication {sqlwhere})t WHERE t.rowid > 0 AND t.rowid <= 100" 分页查询
                  var list = _communiactionContext.Ado.SqlQuery<CommunicationDataModel>(sql);
                  int OKCount = list.Where(r => r.State == (int)DataState.ResultOK).Count();
                  int NGCount = list.Where(r => r.State > (int)DataState.ResultOK).Count();
                  Thread.Sleep(2000);
                  UIAction(() =>
                  {
                      CommunicationDataProperty.CommunicationData = list;
                      CommunicationDataProperty.Count = $"总数 {list.Count}";
                      CommunicationDataProperty.OKCount = $"OK总数 {OKCount} ({(list.Count == 0 ? 0 : OKCount / list.Count):P2})";
                      CommunicationDataProperty.NGCount = $"NG总数 {NGCount} ({(list.Count == 0 ? 0 : NGCount / list.Count):P2})";
                      CommunicationDataProperty.Loading = false;
                  });
              });
        }
        #endregion

        #region ManualUpLoad
        private DelegateCommand _ManualUpLoadCommand;
        public DelegateCommand ManualUpLoadCommand => _ManualUpLoadCommand ??= new DelegateCommand(ManualUpLoadExecute);
        private void ManualUpLoadExecute()
        {
            CommunicationDataProperty.ManualAnimation = true;
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                var CommunicationDatas = CommunicationDataProperty.CommunicationData.Where(r => r.IsSelected);
                _aggregator.GetEvent<UpLoadEvent>().Publish(new UpLoadMessageModel { CommunicationDatas = CommunicationDataProperty.CommunicationData.Where(r => r.IsSelected), IsResult = false });
            });
        }
        #endregion
        public static void UIAction(Action action)//在主线程外激活线程方法
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
        }
        #endregion
    }
}
