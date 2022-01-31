﻿using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.Services.Services
{

    public class OrderServices :BaseService, IOrderServices
    {
        public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService, IUserRepository userRepository) 
            : base(userRepository)
        {
            _orderRepository = orderRepository;
        }
        
        public IOrderRepository _orderRepository { get; set; }
        public IExternalOrderService _externalOrderService { get; set; }
        public IUserRepository _userRepository { get; set; }

        public AddOrderResult Add(AddOrderModel addOrderModel)
        {
            var result = _orderRepository.Add(addOrderModel);
            return new AddOrderResult() { Message = "ok", IsSuccess = result, OrderId = Guid.NewGuid().ToString() };
            var AddOrderResult = _externalOrderService.Add(addOrderModel);

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
                //TOTO log
            }
        }

      

        public OrdersList AllActive(ViewBaseModel addOrderModel)
        {
            return  HttpGetRequest<OrdersList>("");
        }

        public OrdersList All(ViewBaseModel addOrderModel)
        {
            return HttpGetRequest<OrdersList>("");

        }




    }
}
