using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Izi.Online.ViewModels.Trades;

namespace Iz.Online.Services.IServices
{
    public interface IUserService
    {
        List<string> UserHubsList(string UserId);
        List<Trade> Trades(ViewBaseModel viewBaseMode);
    }
}
