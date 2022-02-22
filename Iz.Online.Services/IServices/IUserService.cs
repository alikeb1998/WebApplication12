using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Izi.Online.ViewModels.Trades;
using Asset = Izi.Online.ViewModels.Orders.Asset;
using Izi.Online.ViewModels.Users;

namespace Iz.Online.Services.IServices
{
    public interface IUserService
    {
        string _token { get; set; }
        void SetUserHub(string UserId, string hubId);

        UsersHubIds UserHubsList(string UserId);
        ResultModel<List<Asset>> AllAssets();
        ResultModel<Wallet>  Wallet();
        List<Izi.Online.ViewModels.AppConfigs> AppConfigs();

        ResultModel<string> GetUserLocalToken(string  omsId);
    }
}
