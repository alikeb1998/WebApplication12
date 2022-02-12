using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;

namespace Iz.Online.Services.IServices
{
    public interface ITradeServices
    {
        ResultModel<List<Trade>> Trades();
    }
}
