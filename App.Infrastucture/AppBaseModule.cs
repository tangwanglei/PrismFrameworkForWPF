using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastucture
{
    /// <summary>
    /// 所有Module需继承此类，可实现OnInitialized或者RegisterTypes来构建Region
    /// </summary>
    public abstract class AppBaseModule : IModule
    {
        /// <summary>
        /// 初始化容器提供类
        /// </summary>
        /// <param name="containerProvider"></param>
        public abstract void OnInitialized(IContainerProvider containerProvider);

        /// <summary>
        /// 注册容器内容页
        /// </summary>
        /// <param name="containerRegistry"></param>
        public abstract void RegisterTypes(IContainerRegistry containerRegistry);
    }
}
