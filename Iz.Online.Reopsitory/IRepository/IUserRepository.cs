using Iz.Online.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Users;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IUserRepository : IBaseRepository
    {
        CustomerInfo GetUserHubs(string userId);
    
        string GetUserLocalToken(string omsId, string omsToken);
        void DeleteConnectionId(string connectionId);
        bool SetUserInfo(CustomerInfo model);
        Izi.Online.ViewModels.AppConfigs ConfigData(string key);
        List<Izi.Online.ViewModels.AppConfigs> ConfigData();
    }
}
