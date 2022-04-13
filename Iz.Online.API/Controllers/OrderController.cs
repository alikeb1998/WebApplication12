using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Izi.Online.ViewModels.Orders;
using Iz.Online.Services.IServices;

using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Reports;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]

    public class OrderController : BaseApiController
    {
        #region ctor

        private readonly IOrderServices _orderService;
        private readonly IExternalOrderService _externalOrderService;

        public IHubContext<MainHub> _hubContext;

        public OrderController(IOrderServices orderServices, IHubContext<MainHub> hubContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _orderService = orderServices;
            _hubContext = hubContext;
            _orderService._externalOrderService.Token = _token_;


        }

        #endregion

        [HttpGet("OrderState")]
        public IActionResult OrderStates()
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


            return Ok(states);
        }

        // add order
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddOrderModel addOrderModel)
        {
            var result = await _orderService.Add(addOrderModel);
            return new Respond<AddOrderResult>().ActionRespond(result);
        }

        //get a list of all active orders.
        [HttpGet("all/active")]
        public async Task<IActionResult> AllActive()
        {
            var result = await _orderService.AllActive();
            return new Respond<List<ActiveOrder>>().ActionRespond(result);
        }

        //get a list of all active orders.
        [HttpPost("all/activePaged")]
        public async Task<IActionResult> AllActivePaged([FromBody] OrderFilter filter)
        {
            var result = await _orderService.AllActivePaged(filter);
            return new Respond<OrderReport>().ActionRespond(result);
        }

        //update & edit an order.
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOrder model)
        {
            var result = await _orderService.Update(model);
            return new Respond<UpdatedOrder>().ActionRespond(result);
        }

        //cancel an order.
        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelOrder model)
        {
            var result = await _orderService.Cancel(model);
            return new Respond<CanceledOrder>().ActionRespond(result);
        }

        //get history of all orders.
        [HttpPost("History")]
        public async Task<IActionResult> AllSortedOrder([FromBody] AllOrderCustomFilter filter)
        {
            var result = await _orderService.AllSortedOrder(filter);
            return new Respond<AllOrderReport>().ActionRespond(result);

        }

    }
}
