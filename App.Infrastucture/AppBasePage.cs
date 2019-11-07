using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastucture
{
    /// <summary>
    /// 所有View都需要继承这个类
    /// </summary>
    public abstract class AppBasePage : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// 获取PageName
        /// </summary>
        public virtual string GetPageName()
        {
            return GetType().Name;
        }
    }
}
