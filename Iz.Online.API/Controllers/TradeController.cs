using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using model = Izi.Online.ViewModels.Trades;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class TradeController:BaseApiController
    {
        #region ctor
        public ITradeServices _tradeServices { get; set; }


        public IHubContext<CustomersHub> _hubContext;

        public TradeController(ITradeServices tradeServices, IHubContext<CustomersHub> hubContext)
        {
            _tradeServices = tradeServices; //new OrderServices();
            _hubContext = hubContext;
        }
        #endregion

        [HttpPost("dailyTrades")]

        public ResultModel<List<model.Trade>> Trades([FromBody] ViewBaseModel model)
        {
            var result = _tradeServices.Trades(model);
            return new ResultModel<List<model.Trade>>(result);

        }
    }
}
