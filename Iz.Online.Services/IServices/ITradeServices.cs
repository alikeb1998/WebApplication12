using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;

namespace Iz.Online.Services.IServices
{
    public interface ITradeServices
    {
        string Id { get; set; }

        ResultModel<List<Trade>> Trades();

        IExternalTradeService externalTradeService { get;  }
    }


}
