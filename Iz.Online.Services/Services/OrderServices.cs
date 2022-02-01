using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;

namespace Iz.Online.Services.Services
{
     public class OrderServices : BaseService, IOrderServices
     {
          public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService,
               IUserRepository userRepository)
               : base(userRepository)
          {
               _orderRepository = orderRepository;
          }

          public IOrderRepository _orderRepository { get; set; }
          public IExternalOrderService _externalOrderService { get; set; }
          public IUserRepository _userRepository { get; set; }

          public AddOrderResult Add(AddOrderModel addOrderModel)
          {
               //09:00


               var AddOrderResult = _externalOrderService.Add(addOrderModel);

               //http://192.168.72.54:8080/order/all 
               var getAllModel = new GetAll()
               {
                    Id = AddOrderResult.order.id,
                    Isr = AddOrderResult.order.isr
               };
               var ordersList = All(getAllModel);

               //09:02

               var result = _orderRepository.Add(addOrderModel);


               if (AddOrderResult.statusCode == 0)
               {
                    return new AddOrderResult()
                    {
                         IsSuccess = true,
                         OrderId = Guid.NewGuid().ToString(),
                    };
               }
               else
               {
                    return new AddOrderResult()
                    {
                         IsSuccess = false,
                         OrderId = "error",
                         Message = AddOrderResult.message
                    };
                    
               }
          }

       

          public ActiveOrdersResult AllActive(ViewBaseModel viewBaseModel)
          {
               var activeOrders = _externalOrderService.GetAllActives(viewBaseModel);
               return activeOrders;
          }

          public GetAllResult All(GetAll getAllModel)
          {
               var allResult = _externalOrderService.GetAll(getAllModel);
               return allResult;

          }
     }
}