using Iz.Online.Entities;
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
        void SetToken(TokenStore token);
        string GetToken();

        List<AppConfigs> GetAppConfigs();

         AppConfigs GetAppConfigs(string key);

    }
}
