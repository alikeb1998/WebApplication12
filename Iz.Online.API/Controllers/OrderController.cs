﻿using Confluent.Kafka;
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

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]

    public class OrderController : BaseApiController
    {
        #region ctor

        public IOrderServices _orderServices { get; set; }


        public IHubContext<CustomersHub> _hubContext;

        public OrderController(IOrderServices orderServices, IHubContext<CustomersHub> hubContext)
        {
            _orderServices = orderServices; //new OrderServices();
            _hubContext = hubContext;
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

                // handle consumed message.


                //Console.WriteLine(tt);
                //consumer.Close();
            }

        }


        [HttpPost("add")]
        
        public ResultModel<AddOrderResult> Add([FromBody] AddOrderModel addOrderModel)
        {
            var result = _orderServices.Add(addOrderModel);
            return result;
        }

        //get a list of all active orders.
        [HttpPost("all/active")]       
        public ResultModel<List<ActiveOrder>> AllActive([FromBody] ViewBaseModel addOrderModel)
        {
            var result = _orderServices.AllActive(addOrderModel);
            return result;

        }

        //update & edit an order.
        [HttpPost("update")]
        public ResultModel<UpdatedOrder> Update([FromBody] UpdateOrder model)
        {
            var result = _orderServices.Update(model);
            return result;

        }

        //cancel an order.
        [HttpPost("cancel")]
        public ResultModel<CanceledOrder> Cancel([FromBody] CancelOrder model)
        {
            var result = _orderServices.Cancel(model);
            return result;

        }

        //[HttpPost("order/all")]
        //public ResultModel<OmsModels.ResponsModels.Order.AddOrderResult> All([FromBody] ViewBaseModel addOrderModel)
        //{
        //    var result = _orderServices.All(addOrderModel);
        //    return new ResultModel<OmsModels.ResponsModels.Order.AddOrderResult>(result);
        //}
    }
}
