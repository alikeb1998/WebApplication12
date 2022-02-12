using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.Entities;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IUserRepository : IBaseRepository
    {
        List<string> GetUserHubs(string userId);

        List<AppConfigs> GetAppConfigs();

         AppConfigs GetAppConfigs(string key);

    }
}
