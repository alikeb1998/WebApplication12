using System.Security.Cryptography.X509Certificates;
using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using ActiveOrders = Izi.Online.ViewModels.Orders.ActiveOrders;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using OmsResponse = Iz.Online.OmsModels.ResponsModels.Order;
using db = Iz.Online.Entities;
using Asset = Izi.Online.ViewModels.Orders.Asset;

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

        public AddOrderResult Add(AddOrderModel addOrderModel)
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

            dbEntity.OrderId = addOrderResult.order.id;
            dbEntity.Isr = addOrderResult.order.isr;
            dbEntity.StatusCode = addOrderResult.statusCode;

            var allOrders = _externalOrderService.GetAll(new OmsBaseModel()
            {
                Authorization = addOrderModel.Token
            });

            if (allOrders.statusCode != 200)
            {
                throw new Exception();
            }

            //09:02
            var result =
                 allOrders.orders.FirstOrDefault(x =>
                      x.id == addOrderResult.order.id && x.isr == addOrderResult.order.isr);


            dbEntity.OmsQty = result.quantity;
            dbEntity.OmsPrice = result.price;

            Task.Run(async () => _pushService
                .PushOrderAdded(new List<string>() { "as", "as" }, new Izi.Online.ViewModels.Orders.ActiveOrder()));//TODO

            _orderRepository.Add(dbEntity);

            return new AddOrderResult()
            {
                Message = $"{result.state} {result.errorCode}",
                IsSuccess = addOrderResult.statusCode == 200
            };
        }


        public List<ActiveOrder> AllActive(ViewBaseModel viewBaseModel)
        {
            var activeOrders = _externalOrderService.GetAllActives(viewBaseModel);
            var res = activeOrders.Orders.Select(x => new ActiveOrder()
            {
                Quantity = x.quantity,
                ExecutedQ = x.executedQ,
                InstrumentName = x.instrument.name,
                OrderSide = x.orderSide,
                RemainedQ = x.remainedQ,
                ValidityType = x.validityType,
                OrderQtyWait = x.orderQtyWait,
                CreatedAt = x.createdAt,
                State = x.state,
                ValidityInfo = x.validityType != 2 ? null : x.validityInfo,
                ExecutePercent = x.executePercent
            }).ToList();
            return res;
        }

    }
}