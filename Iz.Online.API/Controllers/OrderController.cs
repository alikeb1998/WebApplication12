using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Izi.Online.ViewModels.Orders;
using Iz.Online.Services.IServices;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Reports;
using Iz.Online.ExternalServices.Rest.ExternalService;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]

    public class OrderController : BaseApiController
    {
        #region ctor

        private readonly IOrderServices _orderService;
        private readonly IExternalOrderService _externalOrderService;

        public IHubContext<CustomersHub> _hubContext;

        public OrderController(IOrderServices orderServices, IHubContext<CustomersHub> hubContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _orderService = orderServices;
            _hubContext = hubContext;
            _orderService._externalOrderService.Token = _token_;


        }

        #endregion


        [HttpGet("test")]

        public string OnRefreshInstrumentDetails()
        {
            return "ok";
            var config = new ConsumerConfig
            {

                BootstrapServers = "192.168.72.222:9092",
                GroupId = "N1"
            };


            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var instrumentId = "IRO1FOLD0001-bestLimit";
                consumer.Subscribe(instrumentId);
                while (true)
                {
                    var consumeResult = consumer.Consume();

                    _hubContext.Clients.All.SendAsync("ReceiveMessage", "asa", consumeResult.Value);

                }
            }

        }

        [HttpGet("OrderState")]
        public ResultModel<List<OrderStates>> OrderStates()
        {

            var states = new List<OrderStates>();
            states.Add(new OrderStates { Code = 1, Key = "OrderCancelled", Value = "لغو شده" });
            states.Add(new OrderStates { Code = 2, Key = "OrderCompletelyExecuted", Value = "سفارش به طور کامل اجرا شده است" });
            states.Add(new OrderStates { Code = 3, Key = "OrderError", Value = "خطای هسته معاملات" });
            states.Add(new OrderStates { Code = 4, Key = "OrderExpired", Value = "منقضی شده" });
            states.Add(new OrderStates { Code = 5, Key = "OrderFilled", Value = "انجام شده" });
            states.Add(new OrderStates { Code = 6, Key = "OrderInProgress", Value = "در حال انتظار" });
            states.Add(new OrderStates { Code = 7, Key = "OrderInQueue", Value = "در صف" });
            states.Add(new OrderStates { Code = 8, Key = "OrderInQueuePendingForCancel", Value = "در صف در انتظار تایید لغو" });
            states.Add(new OrderStates { Code = 9, Key = "OrderInQueuePendingForModify", Value = "در صف در انتظار تایید ویرایش" });
            states.Add(new OrderStates { Code = 10, Key = "OrderPartial", Value = "قسمتی انجام شده" });
            states.Add(new OrderStates { Code = 11, Key = "OrderRejected", Value = "رد شده" });


            return new ResultModel<List<OrderStates>>(states);
        }
        // add order
        [HttpPost("add")]
        public ResultModel<AddOrderResult> Add([FromBody] AddOrderModel addOrderModel)
        {
            var result = _orderService.Add(addOrderModel);
            return result;
        }

        //get a list of all active orders.
        [HttpGet("all/active")]
        public ResultModel<List<ActiveOrder>> AllActive()
        {
            var result = _orderService.AllActive();
            return result;

        }

        //get a list of all active orders.
        [HttpPost("all/activePaged")]
        public ResultModel<OrderReport> AllActivePaged([FromBody] OrderFilter filter)
        {
            var result = _orderService.AllActivePaged(filter);
            return result;

        }

        //update & edit an order.
        [HttpPost("update")]
        public ResultModel<UpdatedOrder> Update([FromBody] UpdateOrder model)
        {
            var result = _orderService.Update(model);
            return result;

        }

        //cancel an order.
        [HttpPost("cancel")]
        public ResultModel<CanceledOrder> Cancel([FromBody] CancelOrder model)
        {
            var result = _orderService.Cancel(model);
            return result;

        }
        //get history of all orders.
        [HttpPost("History")]
        public ResultModel<AllOrderReport> AllSortedOrder([FromBody] AllOrderCustomFilter filter)
        {
            var result = _orderService.AllSortedOrder(filter);
            return result;

        }

    }
}
