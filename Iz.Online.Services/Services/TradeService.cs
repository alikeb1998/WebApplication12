using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.Services
{
    public  class TradeService : ITradeServices
    {
      
        public IExternalTradeService _externalTradeService { get; set; }

        public TradeService(IExternalTradeService externalTradeService)
        {
            _externalTradeService = externalTradeService;
        }

        public ResultModel<List<Trade>> Trades(ViewBaseModel viewBaseMode)
        {
            var trades = _externalTradeService.Trades(viewBaseMode);

            if (!trades.IsSuccess)
                return new ResultModel<List<Trade>>(null, false, trades.Message, trades.StatusCode);
            
            var allTrades = trades.Model.Trades.Where(t => t.TradedAt == DateTime.Today).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.Order.state,
                OrderSide = x.Order.orderSide,
                ExecutedQ = x.Order.executedQ,
                TradedAt = x.TradedAt
            }).ToList();

            return new ResultModel<List<Trade>>(allTrades);
        }
    }
}
