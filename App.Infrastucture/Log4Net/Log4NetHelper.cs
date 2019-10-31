using App.Infrastucture.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastucture.Log
{
    public class Log4NetHelper : ILog
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public void WriteErrorLog(string log)
        {
            throw new NotImplementedException();
        }

        public void WriteInfoLog(string log)
        {
            throw new NotImplementedException();
        }
    }
}
