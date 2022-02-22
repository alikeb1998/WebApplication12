using Iz.Online.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.Entities;
using Izi.Online.ViewModels.Users;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IUserRepository : IBaseRepository
    {
        UsersHubIds GetUserHubs(string userId);
    
        List<AppConfigs> GetAppConfigs();
        string GetUserLocalToken(string omsId, string omsToken);
        void SetUserHub(string UserId, string hubId);
    }
}
