using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalTradeService : BaseService, IExternalTradeService
    {
        public ExternalTradeService(IBaseRepository baseRepository) : base(baseRepository)
        {
           
        }
        public string Id { get; set; }

        public ResultModel<TradesList> Trades()
        {
            var list = HttpGetRequest<TradesList>("trade/all");
          
                return new ResultModel<TradesList>(list, list.statusCode == 200, list.clientMessage, list.statusCode);
            
        }
    }
}
