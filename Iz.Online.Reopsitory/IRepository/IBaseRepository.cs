using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.Entities;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IBaseRepository
    {

        string GetOmsToken(string token);
        void LogException(Exception exception);
        AppConfigs GetAppConfigs(string key);
        bool LocalTokenIsValid(string token);
    }
}
