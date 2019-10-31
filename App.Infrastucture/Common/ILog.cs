using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastucture.Common
{
    public interface ILog
    {
        void WriteInfoLog(string log);

        void WriteErrorLog(string log);
    }
}
