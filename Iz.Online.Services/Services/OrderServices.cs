using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Files;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;

using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using db = Iz.Online.Entities;
using UpdateOrder = Izi.Online.ViewModels.Orders.UpdateOrder;
using UpdatedOrder = Izi.Online.ViewModels.Orders.UpdatedOrder;
using CanceledOrder = Izi.Online.ViewModels.Orders.CanceledOrder;
using CancelOrderModel = Izi.Online.ViewModels.Orders.CancelOrder;
using CancelOrder = Iz.Online.OmsModels.InputModels.Order.CancelOrder;

namespace Iz.Online.Services.Services
{
    public class OrderServices : IOrderServices
    {

        #region ctor
        public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService, IPushService pushService)
        {
            _orderRepository = orderRepository;
            _externalOrderService = externalOrderService;
            _pushService = pushService;
        }


        private readonly IOrderRepository _orderRepository;
        private readonly IExternalOrderService _externalOrderService;
        private readonly IPushService _pushService;

        #endregion

        public ResultModel<AddOrderResult> Add(AddOrderModel addOrderModel)
        {

            //09:00
            var dbEntity = new db.Orders()
            {
                OrderSide = addOrderModel.OrderSide,
                OrderType = addOrderModel.OrderType,
                DisclosedQuantity = addOrderModel.DisclosedQuantity,
                InstrumentId = addOrderModel.InstrumentId,
                ValidityDate = addOrderModel.ValidityDate,
                ValidityType = addOrderModel.ValidityType,
                Quantity = addOrderModel.Quantity,
                CreateOrderDate = DateTime.Now,
                Price = addOrderModel.Price,

            };

            var addOrderResult = _externalOrderService.Add(addOrderModel);

            dbEntity.OmsResponseDate = DateTime.Now;

            dbEntity.OrderId = addOrderResult.Model.order.id;
            dbEntity.Isr = addOrderResult.Model.order.isr;
            dbEntity.StatusCode = addOrderResult.Model.statusCode;

            var allOrders = _externalOrderService.GetAll();

            //09:02

            var result =
                 allOrders.Model.orders.FirstOrDefault(x =>
                      x.id == addOrderResult.Model.order.id && x.isr == addOrderResult.Model.order.isr);


            dbEntity.OmsQty = result.quantity;
            dbEntity.OmsPrice = result.price;

            Task.Run(async () => _pushService
                .PushOrderAdded(new List<string>() { "as", "as" }, new ActiveOrder()));//TODO

            _orderRepository.Add(dbEntity);

            if (addOrderResult.StatusCode != 200)
                return new ResultModel<AddOrderResult>(null, false, addOrderResult.Message, addOrderResult.StatusCode);


            return new ResultModel<AddOrderResult>(new AddOrderResult()
            {
                Message = $"{result.state} {result.errorCode}",
                IsSuccess = addOrderResult.StatusCode == 200
            });
        }

        public ResultModel<List<ActiveOrder>> AllActive()
        {
            var activeOrders = _externalOrderService.GetAllActives();
            if (activeOrders.StatusCode != 200)
                return new ResultModel<List<ActiveOrder>>(null, false, activeOrders.Message, activeOrders.StatusCode);

            var result = activeOrders.Model.Orders.Select(x => new ActiveOrder()
            {
                Quantity = x.quantity,
                ExecutedQ = x.executedQ,
                InstrumentName = x.instrument.name,
                OrderSide = x.orderSide,
                OrderSideText = x.orderSide == 1 ? "فروش" : "خرید",
                RemainedQ = x.remainedQ,
                ValidityType = x.validityType,
                OrderQtyWait = x.orderQtyWait,
                CreatedAt = x.createdAt,
                
                State = x.state,
                StateText = EnumHelper.OrderStates(x.state),

                NscCode = x.instrument.code,
                InstrumentId = x.instrument.id,
                
                ValidityInfo = x.validityType != 2 ? null : x.validityInfo,
                ExecutePercent = x.executePercent
            }).ToList();

            return new ResultModel<List<ActiveOrder>>(result);
        }

        public ResultModel<UpdatedOrder> Update(UpdateOrder model)
        {
            var dbEntity = new db.Orders() { };

            var respond = _externalOrderService.Update(new OmsModels.InputModels.Order.UpdateOrder()
            {
                InstrumentId = model.InstrumentId,
                Price = model.Price,
                Quantity = model.Quantity,
                ValidityDate = model.ValidityDate,
                ValidityType = model.ValidityType,
            });

            //  _orderRepository.Update(dbEntity);

            var result = new UpdatedOrder()
            {

            };

            if (respond.StatusCode != 200)
                return new ResultModel<UpdatedOrder>(null, false, respond.Message, respond.StatusCode);

            return new ResultModel<UpdatedOrder>(result);
        }

        public ResultModel<CanceledOrder> Cancel(CancelOrderModel model)
        {
            var dbEntity = new db.Orders();

            var respond = _externalOrderService.Cancel(new CancelOrder()
            {
                InstrumentId = model.InstrumentId

            });

            var result = new CanceledOrder()
            {

            };

            if (respond.StatusCode != 200)
                return new ResultModel<CanceledOrder>(null, false, respond.Message, respond.StatusCode);

            return new ResultModel<CanceledOrder>(result);

        }

    }
}