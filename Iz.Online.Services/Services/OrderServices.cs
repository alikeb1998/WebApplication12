using System.Security.Cryptography.X509Certificates;
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

        public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService)
        {
            _orderRepository = orderRepository;
            _externalOrderService = externalOrderService;
        }

        public IOrderRepository _orderRepository { get; set; }
        public IExternalOrderService _externalOrderService { get; set; }

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
           
            var addOrderResult = _externalOrderService.Add(addOrderModel );

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
        //public List<Asset> AllAssets(ViewBaseModel viewBaseModel)
        //{
        //    var assets = _externalOrderService.GetAllAssets(viewBaseModel);
        //    var res = assets.Assets.Select(x => new Asset() 
        //    {
        //    Name = x.Instrument.name,
        //    LastPrice = x.LastPrice,
        //    TradeableQuantity = x.TradeableQuantity,
        //    Gav = x.Gav,
        //    AvgPrice = x.AvgPrice,
        //    FianlAmount = x.FinalAmount,
        //    ProfitAmount = x.ProfitAmount,
        //    ProfitPercent = x.ProfitPercent,
        //    SellProfit = x.SellProfit
        //    }).ToList();

        //    return res;
        //}

        
        //public List<OmsModels.ResponsModels.Order.AddOrderResult> All(GetAll getAllModel)
        //{
        //     var allResult = _externalOrderService.GetAll(getAllModel);
        //     return allResult;

        //}
    }
}