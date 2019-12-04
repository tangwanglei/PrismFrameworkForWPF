
using App.Infrastucture;
using App.Pages.View;
using Prism.Ioc;

namespace App.Pages
{
    public class AppPagesModule : AppBaseModule
    {
        public override void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoadingPage>("LoadingPage");
        }
    }
}
