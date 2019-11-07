using App.Infrastucture;
using AppLogin.View;
using Prism.Ioc;
using Prism.Regions;
using System;

namespace AppLogin
{
    public class AppLoginModule : AppBaseModule
    {
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            //var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion(AppRegionType.SurfaceRegion, typeof(LoginPage));

        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginPage>("LoginPage");
        }
    }
}
