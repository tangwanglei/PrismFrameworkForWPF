using App.Infrastucture;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppHome.ViewModel
{
    public class HomePageViewModel : AppBaseViewModel
    {
        #region 属性

        private string _TextBlockData = "这里是主页面";

        public string TextBlockData
        {
            get
            {
                return _TextBlockData;
            }
            set
            {
                _TextBlockData = value;
                RaisePropertyChanged("TextBlockData");
            }
        }

        #endregion

        #region 构造函数

        public HomePageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) :
            base(regionManager, eventAggregator)
        {

        }

        public override void App_ButtonCommand(string command)
        {
            RequestNavigateAndColseCurrentPage(AppRegions.SurfaceRegion, "LoginPage");
        }

        #endregion
    }
}
