using Prism.Ioc;
using Prism.Modularity;
using Rep.Module.Communication.Services.Application.Commands;
using Rep.Module.Communication.Services.Contexts;
using Rep.Module.Communication.Services.Repositorys;
using Rep.Module.Communication.Views;
using SqlSugar;
using System.IO;

namespace Rep.Module.Communication
{
    [Module]
    public class CommunicationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<CreateHandler>();//订阅事件
            containerProvider.Resolve<UpdateStateHandler>();//订阅事件
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConfigView>("ConfigView");
            containerRegistry.RegisterForNavigation<UserLoginView>("UserLoginView");
            containerRegistry.RegisterForNavigation<UserLogOutView>("UserLogOutView");
            containerRegistry.RegisterForNavigation<AlarmView>("AlarmView");
            containerRegistry.RegisterForNavigation<StateChangedView>("StateChangedView");
            containerRegistry.RegisterForNavigation<StationInView>("StationInView");
            containerRegistry.RegisterForNavigation<StationOutView>("StationOutView");
            containerRegistry.RegisterForNavigation<UpLoadTestDataView>("UpLoadTestDataView");
            containerRegistry.RegisterForNavigation<CommunicationDataView>("CommunicationDataView");
            string filePath = @"data\data.db";
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            if (!File.Exists(filePath))
            { 
                File.Create(filePath).Dispose();
            }
            CommuniactionContext userContext = new CommuniactionContext(new ConnectionConfig
            {
                ConnectionString = @"DataSource=data\data.db",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                ConfigId = "sqlite",
            });

            containerRegistry.RegisterInstance(userContext);
            containerRegistry.RegisterSingleton<CreateHandler>();
            containerRegistry.RegisterSingleton<UpdateStateHandler>();
            containerRegistry.RegisterSingleton<ICommunicationRepository, CommunicationRepository>();
        }
    }
}