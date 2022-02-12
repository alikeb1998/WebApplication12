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
        List<string> UserHubsList(string UserId);
        ResultModel<List<Asset>> AllAssets(ViewBaseModel model);
        ResultModel<Wallet>  Wallet(ViewBaseModel model);
        List<Izi.Online.ViewModels.AppConfigs> AppConfigs();
        Izi.Online.ViewModels.AppConfigs AppConfigs(string key);
        ResultModel<Wallet> Wallet(ViewBaseModel model);
        void SetToken(string token);
        string GetToken();


    }
}
