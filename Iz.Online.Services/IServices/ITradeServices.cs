using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;

namespace Iz.Online.Services.IServices
{
    public interface ITradeServices
    {

        ResultModel<List<Trade>> Trades();
        ResultModel<List<Trade>> TradesPaged(TradeFilter filter);
        ResultModel<TradeHistoryReport> History(TradeHistoryFilter filter);

        IExternalTradeService _externalTradeService { get;  }
    }


}
