using Iz.Online.OmsModels.ResponsModels.User;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalTradeService
    { 
        ResultModel<TradesList> Trades();
    }
}
