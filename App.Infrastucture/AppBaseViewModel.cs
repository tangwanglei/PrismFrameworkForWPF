﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace App.Infrastucture
{
    /// <summary>
    /// 所有ViewModel都需要继承这个类
    /// </summary>
    public abstract class AppBaseViewModel : BindableBase, INavigationAware, IJournalAware
    {
        #region 属性

        private IRegionManager regionManager;

        /// <summary>
        /// 区域管理器
        /// </summary>
        protected IRegionManager RegionManager { get { return regionManager; } }


        private IEventAggregator eventAggregator;

        /// <summary>
        /// 事件聚合器
        /// </summary>
        protected IEventAggregator EventAggregator { get { return eventAggregator; } }

        private IRegionNavigationJournal regionNavigationJournal;
        /// <summary>
        /// APP导航(前进，后退)
        /// </summary>
        protected IRegionNavigationJournal RegionNavigationJournal { get { return regionNavigationJournal; } }

        private object navigationParameters;
        /// <summary>
        /// 当导航到当前页传递的参数
        /// </summary>
        protected object NavigationParameters { get { return navigationParameters; } }

        /// <summary>
        /// 当前页面区域名称
        /// </summary>
        protected string CurrentPageRegionType { get; private set; }

        /// <summary>
        /// 当前页面名称
        /// </summary>
        protected string CurrentPageName { get; private set; }

        #endregion

        #region 命令

        private DelegateCommand<string> _AppButtonCommand;
        /// <summary>
        /// 界面Button命令
        /// </summary>
        public DelegateCommand<string> AppButtonCommand
        {
            get
            {
                if (_AppButtonCommand == null)
                {
                    _AppButtonCommand = new DelegateCommand<string>(App_ButtonCommand);
                }
                return _AppButtonCommand;
            }
        }

        /// <summary>
        /// 界面Button执行的虚方法
        /// </summary>
        public virtual void App_ButtonCommand(string command) { }

        #endregion

        #region 构造函数

        /// <summary>
        /// AppBase构造函数,所有的ViewModel都要继承这个接口
        /// </summary>
        /// <param name="_regionManager"></param>
        /// <param name="_eventAggregator"></param>
        public AppBaseViewModel(IRegionManager _regionManager, IEventAggregator _eventAggregator)
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
            CurrentPageRegionType = navigationContext.NavigationService.Region.Name;
            CurrentPageName = navigationContext.Uri.OriginalString;
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
        protected void RequestNavigate(string regionType, string appViewName, NavigationParameters parameters = null)
        {
            RegionManager?.RequestNavigate(regionType, appViewName, parameters);
        }

        /// <summary>
        /// 跳转页面并关闭当前页面
        /// </summary>
        protected void RequestNavigateAndColseCurrentPage(string regionType, string appViewName, NavigationParameters parameters = null)
        {
            RegionManager?.RequestNavigate(regionType, appViewName, parameters);
            if (!string.IsNullOrEmpty(CurrentPageRegionType))
                ViewClose(CurrentPageRegionType);
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="regionType">区域名称</param>
        /// <param name="targetPageName">目标页面名称(如果目标页面不是AppBasePage,则需要填写此值防止误删页面)</param>
        /// <param name="curPage">待关闭的页面</param>
        protected void ViewClose(string regionType, string targetPageName = null, AppBasePage curPage = null)
        {
            var appRegions = regionManager.Regions.Select(x => x.Name).ToList();
            if (!appRegions.Any(x => x.Equals(regionType)))
                return;
            var appRegion = regionManager.Regions[regionType];
            if (curPage == null)
                curPage = regionManager.Regions[regionType].ActiveViews.FirstOrDefault() as AppBasePage;
            if (curPage != null && curPage.GetPageName().Equals(CurrentPageName))
                appRegion.Remove(curPage);
            else if (curPage == null)
            {
                var userPage = regionManager.Regions[regionType].ActiveViews.FirstOrDefault() as UserControl;
                if (userPage != null && userPage.GetType().Name.Equals(targetPageName))
                    appRegion.Remove(userPage);
            }

        }

        /// <summary>
        /// 区别UI线程异步执行耗时函数，例如：导出，导入，保存图片等耗时操作
        /// </summary>
        /// <param name="fun">异步执行的函数(此函数不在UI线程执行，为具体耗时的操作，不可操作UI线程对象)</param>
        /// <param name="completedFun">异步结束时所要执行的函数(此函数依然在UI线程执行，因此可以操作UI线程对象)</param>
        protected void Async(Func<bool> fun, Func<bool> completedFun)
        {
            var asyncFun = new Func<bool>(() =>
            {
                //主要执行的
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    RequestNavigate(AppRegions.SurfaceRegion, AppPages.LoadingPage);
                }), null);
                return fun();
            });
            asyncFun.BeginInvoke((result) => Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ViewClose(AppRegions.SurfaceRegion);
                if (result.IsCompleted)
                    completedFun();
            }), null), null);

        }

        #endregion

        #region 是否在导航历史中，存在这个页面

        public bool bPersistInHistory = false;

        /// <summary>
        /// 是否在导航历史中，存在这个页面
        /// </summary>
        /// <returns></returns>
        public bool PersistInHistory()
        {
            return bPersistInHistory;
        }

        #endregion
    }
}
