using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.API.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Iz.Online.Services.IServices;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.SignalR;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class OrderController : BaseApiController
    {
        #region ctor

        public IOrderServices _orderServices { get; set; }
        //public IUserService _userService { get; set; }

        public IHubContext<CustomersHub> _hubContext;

        public OrderController(IOrderServices orderServices, IUserService userService, IHubContext<CustomersHub> hubContext)
        {
            _orderServices = orderServices; //new OrderServices();
            _hubContext = hubContext;
            //_userService = userService;
        }

        #endregion


        [HttpGet("test")]
        public void OnRefreshInstrumentDetails()
        {

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

                // handle consumed message.


                //Console.WriteLine(tt);
                //consumer.Close();
            }

        }


        [HttpPost("add")]
        public ResultModel<AddOrderResult> Add([FromBody] AddOrderModel addOrderModel)
        {
            var result = _orderServices.Add(addOrderModel);
            return new ResultModel<AddOrderResult>(result);

        }


        [HttpGet("order/all/active")]
        public ResultModel<ActiveOrdersResult> AllActive([FromBody] ViewBaseModel viewBaseModel)
        {
            var result = _orderServices.AllActive(viewBaseModel);
            return new ResultModel<ActiveOrdersResult>(result);
        }


        [HttpGet("order/all")]
        public ResultModel<GetAllResult> All([FromBody] GetAll getAllModel)
        {
            var result = _orderServices.All(getAllModel);
            return new ResultModel<GetAllResult>(result);
        }
        
      
    }
}
