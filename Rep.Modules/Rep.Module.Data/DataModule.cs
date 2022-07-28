using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Rep.Module.Data.Views;

namespace Rep.Module.Data
{
    public class DataModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SelectDataView>("SelectDataView");
        }
    }
}