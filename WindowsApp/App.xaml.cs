using Microsoft.Win32;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace WindowsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {

        #region 程序启动

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            ChangeProcessRenderMode();
            base.OnStartup(e);

            //异常处理
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        /// <summary>
        /// 改变程序呈现方式，改变加速要在OnStartup之前执行，在后面执行将不会生效
        /// </summary>
        void ChangeProcessRenderMode()
        {
            int renderingTier = (System.Windows.Media.RenderCapability.Tier >> 16);
            if (renderingTier == 0)
            {
                DisableHWAcceleration();
                //不支持硬件加速
            }
            else if (renderingTier == 1)
            {
                //支持部分硬件加速
            }
            else
            {
                EnableHWAcceleration();
                //支持硬件加速
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(e.ExceptionObject.ToString());
            }
            catch
            {

            }
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Exception.Message);
            }
            catch { }
            finally
            {
                e.Handled = true;
            }
        }

        #endregion

        /// <summary>
        /// 配置Shell指定的主窗口
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 注册导航页面，可自定义导航页面的名称
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<MainWindow>("MainWindow");
        }

        /// <summary>
        /// 自动在根目录探测模块，可配置探测位置
        /// </summary>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            //return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
            return new DirectoryModuleCatalog() { ModulePath = @".\" };
        }

        /// <summary>
        /// 配置模块目录
        /// </summary>
        /// <param name="moduleCatalog">模块目录管理器</param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            var moduleAType = typeof(IModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleAType.Name,
                ModuleType = moduleAType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
        }

        /// <summary>
        /// 配置ViewModel加载项
        /// 注：VM的命名空间和View的命名空间一致才能自动匹配到
        ///     View 要配置 prism:ViewModelLocator.AutoWireViewModel="True" 才能触发自动匹配
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var namespaces = viewType.Namespace.Split('.');
                var viewModelName = $"{namespaces[0]}.ViewModel.{viewType.Name}ViewModel";
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelFullName = $"{viewModelName}, {viewAssemblyName}";
                return Type.GetType(viewModelFullName);
            });

            // type / type
            //ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), typeof(CustomViewModel));

            // type / factory
            //ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), () => Container.Resolve<CustomViewModel>());

            // generic factory
            //ViewModelLocationProvider.Register<MainWindow>(() => Container.Resolve<CustomViewModel>());

            // ** 指定View 使用 ViewModel
            //ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }

        #region 程序退出

        /// <summary>
        /// 程序退出
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            DisableHWAcceleration();
            base.OnExit(e);
        }


        #endregion

        #region 注册表硬件加速设置

        /// <summary>
        /// 启用硬件加速
        /// </summary>
        void EnableHWAcceleration()
        {
            string subKey = @"SOFTWARE\Microsoft\Avalon.Graphics\";
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(subKey, true);
            if (registryKey == null)
            {
                registryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(subKey);
            }
            if (registryKey.GetValue("DisableHWAcceleration") == null)
            {
                registryKey.SetValue("DisableHWAcceleration", 0, RegistryValueKind.DWord);
            }

            if (registryKey != null && registryKey.GetValue("DisableHWAcceleration").ToString() == "1")
            {
                registryKey.SetValue("DisableHWAcceleration", 0);
            }

            registryKey.Dispose();
        }

        /// <summary>
        /// 关闭硬件加速
        /// </summary>
        public static void DisableHWAcceleration()
        {
            using (RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Avalon.Graphics\", true))
            {
                if (registryKey != null && registryKey.GetValue("DisableHWAcceleration", null) != null && registryKey.GetValue("DisableHWAcceleration").ToString() == "0")
                {
                    registryKey.SetValue("DisableHWAcceleration", 1);
                }
            }
        }


        #endregion

    }
}
