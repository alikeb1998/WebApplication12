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
using Report = Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.Reports;
using System.Linq.Expressions;

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
                RegisterOrderDate = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                OrderSide = addOrderModel.OrderSide,
                OrderType = addOrderModel.OrderType,
                DisclosedQuantity = addOrderModel.DisclosedQuantity,
                InstrumentId = addOrderModel.InstrumentId,
                ValidityDate = addOrderModel.ValidityDate,
                ValidityType = addOrderModel.ValidityType,
                Quantity = addOrderModel.Quantity,
                CreateOrderDate = DateTime.Now,
                Price = addOrderModel.Price,
                CustomerId = addOrderModel.CustomerId,
            };

            var addOrderResult = _externalOrderService.Add(addOrderModel);

            if (!addOrderResult.IsSuccess)
                return new ResultModel<AddOrderResult>(null, false, addOrderResult.Message);

            dbEntity.OmsResponseDate = DateTime.Now;
            dbEntity.OrderId = addOrderResult.Model.order.id;
            dbEntity.Isr = addOrderResult.Model.order.isr;
            dbEntity.StatusCode = addOrderResult.Model.statusCode;

            var allOrders = _externalOrderService.GetAll();
            if (!allOrders.IsSuccess)
                return new ResultModel<AddOrderResult>(null, false, addOrderResult.Message);
            //09:02

            var result =
                 allOrders.Model.orders.FirstOrDefault(x =>
                      x.id == addOrderResult.Model.order.id && x.isr == addOrderResult.Model.order.isr);


            dbEntity.OmsQty = result.quantity;
            dbEntity.OmsPrice = result.price;

            Task.Run(async () => _pushService
                .PushOrderAdded(new List<string>() { "as", "as" }, new ActiveOrder()));//TODO

            _orderRepository.Add(dbEntity);

            if (addOrderResult.StatusCode != 1)
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

            if (activeOrders.Model.Orders.Count == 0)
                return new ResultModel<List<ActiveOrder>>(null, true, activeOrders.Message, activeOrders.StatusCode);

            var result = activeOrders.Model.Orders.Select(x => new ActiveOrder()
            {
                Quantity = (long)x.quantity,
                ExecutedQ = (long)x.executedQ,
                InstrumentName = x.instrument.name,
                OrderSide = x.orderSide,
                OrderSideText = x.orderSide == 1 ? "فروش" : "خرید",
                RemainedQ = x.remainedQ,
                ValidityType = x.validityType,
                OrderQtyWait = x.orderQtyWait,
                CreatedAt = x.createdAt,
                Price = x.price,
                State = x.state,
                // StateText = EnumHelper.OrderStates(x.state),

                NscCode = x.instrument.code,
                InstrumentId = x.instrument.id,

                ValidityInfo = x.validityType != 2 ? null : x.validityInfo,
                ExecutePercent = x.executedQ / x.quantity * 100
            }).ToList();


            return new ResultModel<List<ActiveOrder>>(result);
        }
        public ResultModel<OrderReport> AllActivePaged(OrderFilter filter)
        {
            var activeOrders = _externalOrderService.GetAllActives();
            //if (activeOrders.StatusCode != 1)
            //    return new ResultModel<List<ActiveOrder>>(null, false, activeOrders.Message, activeOrders.StatusCode);

            var result = activeOrders.Model.Orders.Select(x => new ActiveOrder()
            {
                Quantity = (long)x.quantity,
                ExecutedQ = (long)x.executedQ,
                InstrumentName = x.instrument.name,
                OrderSide = x.orderSide,
                OrderSideText = x.orderSide == 1 ? "فروش" : "خرید",
                RemainedQ = x.remainedQ,
                ValidityType = x.validityType,
                OrderQtyWait = x.orderQtyWait,
                CreatedAt = x.createdAt,
                Price = x.price,
                State = x.state,
                // StateText = EnumHelper.OrderStates(x.state),
                NscCode = x.instrument.code,
                InstrumentId = x.instrument.id,
                ValidityInfo = x.validityType != 2 ? null : x.validityInfo,
                ExecutePercent = x.executedQ / x.quantity * 100
            }).ToList();

            for (int i = 50; i > 0; i--)
            {
                var mock = new ActiveOrder() { InstrumentId = i, InstrumentName = $"نماد{i}", CreatedAt = DateTime.Now.AddSeconds(1), ExecutePercent = i, Quantity = i };
                result.Add(mock);
            }
            var a = Filter(result, filter);

            var res = new OrderReport()
            {
                Model = a,
                OrderType = filter.OrderType,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = result.Count,
                OrderSortColumn = filter.OrderSortColumn
            };

            return new ResultModel<OrderReport>(res);
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
        public List<ActiveOrder> Filter(List<ActiveOrder> list, OrderFilter filter)
        {
            var report = new OrderReport()
            {
                Model = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                OrderSortColumn = filter.OrderSortColumn,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = list.Count,
                OrderType = filter.OrderType,
            };

            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderSortColumn)
                    {
                        case OrderSortColumn.Symbol:
                            return report.Model.OrderBy(x => x.InstrumentName).ToList();

                        case OrderSortColumn.Percentage:
                            return report.Model.OrderBy(x => x.ExecutePercent).ToList();

                        case OrderSortColumn.Date:
                            return report.Model.OrderBy(x => x.CreatedAt).ToList();

                        case OrderSortColumn.Volume:
                            return report.Model.OrderBy(x => x.Quantity).ToList();

                    }
                    break;

                case OrderType.DESC:
                    switch (filter.OrderSortColumn)
                    {
                        case OrderSortColumn.Symbol:
                            return report.Model.OrderByDescending(x => x.InstrumentName).ToList();


                        case OrderSortColumn.Percentage:
                            return report.Model.OrderByDescending(x => x.ExecutePercent).ToList();


                        case OrderSortColumn.Date:
                            return report.Model.OrderByDescending(x => x.CreatedAt).ToList();


                        case OrderSortColumn.Volume:
                            return report.Model.OrderByDescending(x => x.Quantity).ToList();

                    }
                    break;

                default:
                    return report.Model.OrderBy(x => x.CreatedAt).ToList();
            }
            return report.Model.OrderBy(x => x.CreatedAt).ToList();
        }
    }
}