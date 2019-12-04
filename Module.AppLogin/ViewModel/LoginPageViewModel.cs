using App.Infrastucture;
using Prism.Regions;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppLogin.ViewModel
{
    public class LoginPageViewModel : AppBaseViewModel
    {
        #region 属性

        private string _TextBlockData = "这里是登陆页面";

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

        public LoginPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) :
            base(regionManager, eventAggregator)
        {

        }

        public override void App_ButtonCommand(string command)
        {
            RequestNavigateAndColseCurrentPage(AppRegions.SurfaceRegion, "HomePage");
        }

        #endregion
    }
}
