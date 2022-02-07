using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Izi.Online.ViewModels.Trades;
using Asset = Izi.Online.ViewModels.Orders.Asset;

namespace Iz.Online.Services.IServices
{
    public interface IUserService
    {
        List<string> UserHubsList(string UserId);
        List<Asset> AllAssets(ViewBaseModel viewBaseMode);


    }
}
