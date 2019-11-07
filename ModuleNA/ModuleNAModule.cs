using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleNA
{
    public class ModuleNAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion("RightRegion", typeof(MessageList));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewNA>("CustormNameA");
            containerRegistry.RegisterForNavigation<ViewNB>();
        }
    }
}
