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

namespace Iz.Online.Services.Services
{
     public class OrderServices : BaseService, IOrderServices
     {
          public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService,
               IUserRepository userRepository)
               : base(userRepository)
          {
               _orderRepository = orderRepository;
               _externalOrderService = externalOrderService;
          }

          public IOrderRepository _orderRepository { get; set; }
          public IExternalOrderService _externalOrderService { get; set; }
          public IUserRepository _userRepository { get; set; }

          public Izi.Online.ViewModels.Orders.AddOrderResult Add(AddOrderModel addOrderModel)
          {
               //09:00
               var dbEntity = new db.Orders();
               dbEntity = addOrderModel;
               dbEntity.CreateOrderDate = DateTime.Now;

               var addOrderResult = _externalOrderService.Add(addOrderModel);

               dbEntity.OmsResponseDate = DateTime.Now;
               dbEntity.OrderId = addOrderResult.order.id;
               dbEntity.Isr = addOrderResult.order.isr;
               dbEntity.StatusCode = addOrderResult.statusCode;


               var allOrders = _externalOrderService.GetAll(new OmsBaseModel());
               if (allOrders.statusCode != 200)
               {
                    throw new Exception();
               }

               //09:02
               var result =
                    allOrders.orders.FirstOrDefault(x =>
                         x.id == addOrderResult.order.id && x.isr == addOrderResult.order.isr);


               dbEntity.OmsQty = result.quantity;
               dbEntity.Price = result.price;

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

          //public List<OmsModels.ResponsModels.Order.AddOrderResult> All(GetAll getAllModel)
          //{
          //     var allResult = _externalOrderService.GetAll(getAllModel);
          //     return allResult;

          //}
     }
}