using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Izi.Online.ViewModels.Trades;
using Asset = Izi.Online.ViewModels.Orders.Asset;
using Izi.Online.ViewModels.Users;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.OmsModels.InputModels.Users;

namespace Iz.Online.HubConnectionHandler.IServices
{
    public interface IHubConnationService
    {

        CustomerInfo UserHubsList(string userId);
        void DeleteConnectionId(string connectionId);
    }
}
