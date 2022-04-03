using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using model = Izi.Online.ViewModels.Trades;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.Orders;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class TradeController : BaseApiController
    {
        #region ctor
        public ITradeServices _tradeServices { get; set; }


        public IHubContext<CustomersHub> _hubContext;

        public TradeController(ITradeServices tradeServices, IHubContext<CustomersHub> hubContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _tradeServices = tradeServices; //new OrderServices();
            _hubContext = hubContext;
            _tradeServices._externalTradeService.Token = _token_;

        }
        #endregion

        // show a list of customer today trades.
        [HttpGet("dailyTrades")]
        public IActionResult Trades()
        {
            var result = _tradeServices.Trades();
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
      
        [HttpPost("dailyTradesPaged")]
        public IActionResult TradesPaged([FromBody] TradeFilter filter)
        {
            var result = _tradeServices.TradesPaged(filter);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        //get history of all trades.
        [HttpPost("History")]
        public IActionResult History([FromBody] TradeHistoryFilter filter)
        {
            var result = _tradeServices.History(filter);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };

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
