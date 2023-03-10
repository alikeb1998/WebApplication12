using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
 
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using Izi.Online.ViewModels;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Trades;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.HubHandler;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class TradeController : BaseApiController
    {
        #region ctor
        public ITradeServices _tradeServices { get; set; }


        public IHubContext<MainHub> _hubContext;

        public TradeController(ITradeServices tradeServices, IHubContext<MainHub> hubContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _tradeServices = tradeServices; //new OrderServices();
            _hubContext = hubContext;
            _tradeServices._externalTradeService.Token = _token_;

        }
        #endregion

        // show a list of customer today trades.
        [HttpGet("dailyTrades")]
        public async Task<IActionResult> Trades()
        {
            var result =  await _tradeServices.Trades();
            return new Respond<List<Trade>>().ActionRespond(result);
        }
      
        [HttpPost("dailyTradesPaged")]
        public async Task<IActionResult> TradesPaged([FromBody] TradeFilter filter)
        {
            var result = await _tradeServices.TradesPaged(filter);
            return new Respond<List<Trade>>().ActionRespond(result);
        }

        //get history of all trades.
        [HttpPost("History")]
        public async Task<IActionResult> History([FromBody] TradeHistoryFilter filter)
        {
            var result = await _tradeServices.History(filter);
            return new Respond<TradeHistoryReport>().ActionRespond(result);

        }

        [HttpGet("TradeState")]
        public ResultModel<List<OrderStates>> TradeStates()
        {
            var states = new List<OrderStates>();
            states.Add(new OrderStates { Code = 1, Key = "Cancelled", Value = "لغو شده" });
            states.Add(new OrderStates { Code = 2, Key = "Executed", Value = "معامله شده" });
            states.Add(new OrderStates { Code = 3, Key = "ExternallyCreated", Value = "معامله توسط ناظر بازار" });
         
            return new ResultModel<List<OrderStates>>(states);
        }
    }
}
