using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;

namespace App.Infrastucture
{
    /// <summary>
    /// 所有ViewModel都需要继承这个类
    /// </summary>
    public class AppBase : BindableBase, INavigationAware
    {
        #region 属性

        private IRegionManager regionManager;

        /// <summary>
        /// 区域管理器
        /// </summary>
        public IRegionManager RegionManager { get { return regionManager; } }


        private IEventAggregator eventAggregator;

        /// <summary>
        /// 事件聚合器
        /// </summary>
        public IEventAggregator EventAggregator { get { return eventAggregator; } }

        private IRegionNavigationJournal regionNavigationJournal;
        /// <summary>
        /// APP导航(前进，后退)
        /// </summary>
        public IRegionNavigationJournal RegionNavigationJournal { get { return regionNavigationJournal; } }

        private object navigationParameters;
        /// <summary>
        /// 当导航到当前页传递的参数
        /// </summary>
        public object NavigationParameters { get { return navigationParameters; } }

        #endregion

        #region 构造函数

        /// <summary>
        /// AppBase构造函数,所有的ViewModel都要继承这个接口
        /// </summary>
        /// <param name="_regionManager"></param>
        /// <param name="_eventAggregator"></param>
        public AppBase(IRegionManager _regionManager, IEventAggregator _eventAggregator)
        {
            regionManager = _regionManager;
            eventAggregator = _eventAggregator;
        }

        #endregion

        #region 接口方法

        /// <summary>
        /// 定义一个当导航到当前页面的委托
        /// </summary>
        /// <param name="navigationContext"></param>
        public delegate void App_OnNavigatedTo_Event(NavigationContext navigationContext);

        /// <summary>
        /// 当导航到当前页面的委托事件
        /// </summary>
        public App_OnNavigatedTo_Event App_OnNavigatedTo;

        /// <summary>
        /// 当导航到当前页面的时候
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            regionNavigationJournal = navigationContext.NavigationService.Journal;
            navigationParameters = navigationContext.Parameters["param"];
            App_OnNavigatedTo?.Invoke(navigationContext);
        }

        /// <summary>
        /// 是否每次导航到当前页都生成新页面(默认返回true，即不新生成页面)
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }


        #endregion

        #region App方法

        /// <summary>
        /// 前进
        /// </summary>
        protected void GoForward()
        {
            regionNavigationJournal?.GoForward();
        }

        /// <summary>
        /// 返回
        /// </summary>
        protected void GoBack()
        {
            regionNavigationJournal?.GoBack();
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        protected void RequestNavigate(string regionType, string appViewName, NavigationParameters parameters)
        {
            RegionManager?.RequestNavigate(regionType, appViewName, parameters);
        }

        /// <summary>
        /// 跳转页面并关闭当前页面
        /// </summary>
        protected void RequestNavigateAndCloseCurrentPage(string regionType, string appViewName, NavigationParameters parameters)
        {
            RegionManager?.RequestNavigate(regionType, appViewName, parameters);
            ViewClose(regionType);
        }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        /// <param name="curPage"></param>
        protected void ViewClose(string regionType, AppBasePage curPage = null)
        {
            try
            {
                if (curPage == null)
                {
                    curPage = (AppBasePage)regionManager.Regions[regionType].ActiveViews.FirstOrDefault();
                    if (curPage != null)
                    {
                        regionManager.Regions[regionType].Remove(curPage);
                    }
                }
                else
                {
                    regionManager.Regions[regionType].Remove(curPage);
                }
            }
            catch (Exception ex)
            {
                //WMSLog.WriteLog(LogType.Error, string.Format("Tab子页关闭失败，原因：{0}", ex.ToString()));
            }
        }

        #endregion

    }
}
