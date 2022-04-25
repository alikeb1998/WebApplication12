
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
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.HubHandler;

namespace Iz.Online.Services.Services
{
    public class OrderServices : IOrderServices
    {

        #region ctor


        private readonly IOrderRepository _orderRepository;
        private IHubUserService _hubUserService;

        public IExternalOrderService _externalOrderService { get; }
        public ICacheService _cacheService { get; }

        public OrderServices(IOrderRepository orderRepository, IExternalOrderService externalOrderService, ICacheService cacheService, IHubUserService hubUserService)
        {
            _orderRepository = orderRepository;
            _externalOrderService = externalOrderService;
            _cacheService = cacheService;
            _hubUserService = hubUserService;

        }


        #endregion

        public async Task<ResultModel<AddOrderResult>> Add(AddOrderModel addOrderModel)
        {
            var instrumentData = _cacheService.InstrumentData((int)addOrderModel.InstrumentId);

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
                CustomerId = addOrderModel.TradingId,
            };
            addOrderModel.InstrumentId = instrumentData.InstrumentId;

            var addOrderResult = await _externalOrderService.Add(addOrderModel);

            if (!addOrderResult.IsSuccess)
                return new ResultModel<AddOrderResult>(null, addOrderResult.StatusCode == 200, addOrderResult.Message, addOrderResult.StatusCode);
            dbEntity.OmsResponseDate = DateTime.Now;
            dbEntity.OrderId = addOrderResult.Model.order.id;
            dbEntity.Isr = addOrderResult.Model.order.isr;
            dbEntity.StatusCode = addOrderResult.Model.statusCode;

            var allOrders = await _externalOrderService.GetAll();
            if (!allOrders.IsSuccess)
                return new ResultModel<AddOrderResult>(null, allOrders.StatusCode == 200, allOrders.Message, allOrders.StatusCode);

            var result =
                 allOrders.Model.orders.FirstOrDefault(x =>
                      x.id == addOrderResult.Model.order.id && x.isr == addOrderResult.Model.order.isr);

            dbEntity.OmsQty = result.quantity;
            dbEntity.OmsPrice = result.price;

            _orderRepository.Add(dbEntity);

            return new ResultModel<AddOrderResult>(new AddOrderResult()
            {
                Message = $"{result.state} {result.errorCode}",
                IsSuccess = addOrderResult.StatusCode == 200
            });
        }

        public async Task<ResultModel<List<ActiveOrder>>> AllActive()
        {
            var instruments = _cacheService.InstrumentData();

            var activeOrders = await _externalOrderService.GetAllActives();
            if (activeOrders.StatusCode != 200 || activeOrders.Model.Orders.Count == 0)
                return new ResultModel<List<ActiveOrder>>(null, activeOrders.StatusCode == 200, activeOrders.Message, activeOrders.StatusCode);


            var allActiveOrders = activeOrders.Model.Orders.Select(x => new ActiveOrder()
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
                NscCode = x.instrument.code,
                InstrumentId = _cacheService.GetLocalInstrumentIdFromOmsId(x.instrument.id),
                OrderId = x.id,
                ValidityInfo = x.validityType != 2 ? null : x.validityInfo,
                ExecutePercent = x.executedQ / x.quantity * 100
            }).ToList();

            var result = from trade in allActiveOrders
                         join instrument in instruments on trade.InstrumentId equals instrument.InstrumentId
                         select new ActiveOrder()
                         {
                             Quantity = trade.Quantity,
                             ExecutedQ = trade.ExecutedQ,
                             InstrumentName = trade.InstrumentName,
                             OrderSide = trade.OrderSide,
                             OrderSideText = trade.OrderSideText,
                             RemainedQ = trade.RemainedQ,
                             ValidityType = trade.ValidityType,
                             OrderQtyWait = trade.OrderQtyWait,
                             CreatedAt = trade.CreatedAt,
                             Price = trade.Price,
                             State = trade.State,
                             NscCode = trade.NscCode,
                             ValidityInfo = trade.ValidityInfo,
                             ExecutePercent = trade.ExecutePercent,
                             InstrumentId = (int)instrument.Id,
                             StateText = trade.StateText,
                             OrderId = trade.OrderId,
                         };

            return new ResultModel<List<ActiveOrder>>(result.OrderByDescending(x => x.CreatedAt).ToList());
        }
        public async Task<ResultModel<OrderReport>> AllActivePaged(OrderFilter filter)
        {
            var activeOrders = await _externalOrderService.GetAllActives();

            if (activeOrders.StatusCode != 200 || activeOrders.Model.Orders.Count == 0)
                return new ResultModel<OrderReport>(null, activeOrders.StatusCode == 200, activeOrders.Message, activeOrders.StatusCode);

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
            var a = ActiveOrdersFilter(result, filter);

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

        public async Task<ResultModel<AllOrderReport>> AllSortedOrder(AllOrderCustomFilter filter)
        {
            if (filter.PageNumber == 0 || filter.PageSize == 0)
            {
                return new ResultModel<AllOrderReport>(null, 400);
            }
            var allOrders = await _externalOrderService.GetAll();
            if (allOrders.IsSuccess && allOrders.Model.orders != null && allOrders.Model.orders.Count > 0)
            {
                var result = allOrders.Model.orders.Select(x => new AllOrder()
                {
                    CreatedAt = x.CreatedAt,
                    ExecutedQ = x.ExecutedQ,
                    Id = x.id,
                    price = x.price,
                    Quantity = x.quantity,
                    State = x.state,
                    ValidityType = x.ValidityType,
                    InstrumentName = x.Instrument.name,
                    ValueOfExeCutedQ = x.ExecutedQ * x.price,
                    ValidityTypeText = x.ValidityType switch
                    {
                        1 => "روز",
                        2 => "معتبر تا تاریخ",
                        3 => "معتبر تا لغو",
                        4 => "انجام و حذف",
                        _ => " "
                    },

                    InstrumentId = x.Instrument.id,
                    OrderSide = x.orderSide,
                    ValidityInfo = x.ValidityInfo

                }).OrderByDescending(x => x.CreatedAt).ToList();
                var a = AllOrdersFilter(result, filter);
                if (a != null)
                {
                    a.Model = a.Model.OrderByDescending(x => x.CreatedAt).ToList();
                    return new ResultModel<AllOrderReport>(a);

                }
                return new ResultModel<AllOrderReport>(new AllOrderReport() { Model=new(), TotalCount=0,PageNumber=0,PageSize=0}, 200, "لیست خالی است");
            }
            return new ResultModel<AllOrderReport>(new AllOrderReport() { Model = new(), TotalCount = 0, PageNumber = 0, PageSize = 0 }, 200, "لیست خالی است");
        }

        public async Task<ResultModel<UpdatedOrder>> Update(UpdateOrder model)
        {
            var dbEntity = new db.Orders() { };

            var respond = await _externalOrderService.Update(new OmsModels.InputModels.Order.UpdateOrder()
            {
                InstrumentId = model.InstrumentId,
                Price = model.Price,
                Quantity = model.Quantity,
                ValidityDate = model.ValidityDate,
                ValidityType = model.ValidityType,
            });

            var result = new UpdatedOrder();
            if (respond.StatusCode != 200 || respond.Model == null)
                return new ResultModel<UpdatedOrder>(null, respond.StatusCode == 200, respond.Message, respond.StatusCode);

            return new ResultModel<UpdatedOrder>(result);
        }

        public async Task<ResultModel<CanceledOrder>> Cancel(CancelOrderModel model)
        {
            var dbEntity = new db.Orders();

            var respond = await _externalOrderService.Cancel(new CancelOrder()
            {
                InstrumentId = model.InstrumentId

            });
            if (respond.StatusCode != 200 || respond.Model == null)
                return new ResultModel<CanceledOrder>(null, respond.StatusCode == 200, respond.Message, respond.StatusCode);

            var result = new CanceledOrder();

            return new ResultModel<CanceledOrder>(result);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private List<ActiveOrder> ActiveOrdersFilter(List<ActiveOrder> list, OrderFilter filter)
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
        private AllOrderReport AllOrdersFilter(List<AllOrder> list, AllOrderCustomFilter filter)
        {


            if (filter.From == DateTime.MinValue)
            {
                var a = list.OrderBy(e => e.CreatedAt).ToList();
                filter.From = a[0].CreatedAt;
            }
            if (filter.To == DateTime.MinValue)
            {
                var a = list.OrderByDescending(e => e.CreatedAt).ToList();
                filter.To = a[0].CreatedAt;
            }


            list = list.Where(x => DateTime.Compare(x.CreatedAt, filter.From) >= 0 && DateTime.Compare(filter.To, x.CreatedAt) >= 0).ToList();

            var instrumentList = new List<AllOrder>();
            if (filter.InstrumentId.Count == 0)
            {
                instrumentList.AddRange(list);
            }
            foreach (var f in filter.InstrumentId)
            {
                if (filter.InstrumentId.Count == 1 && f == 0)
                {
                    instrumentList.AddRange(list);
                }
                if (f != 0)
                {
                    var a = list.Where(x => _cacheService.GetLocalInstrumentIdFromOmsId(x.InstrumentId) == f).ToList();
                    instrumentList.AddRange(a);
                }
            }
            if (instrumentList.Count == 0)
            {
                return null;
            }


            if (filter.OrderSide != 0)
            {
                switch (filter.OrderSide)
                {
                    case 1:
                        instrumentList = instrumentList.Where(x => x.OrderSide == 1).ToList();
                        break;

                    case 2:
                        instrumentList = instrumentList.Where(x => x.OrderSide == 2).ToList();
                        break;
                }
            }

            if (filter.State != 0)
            {
                switch (filter.State)
                {
                    case 5:
                        instrumentList = instrumentList.Where(x => x.State == "انجام شده").ToList();
                        break;

                    case 4:
                        instrumentList = instrumentList.Where(x => x.State == "منقضی شده").ToList();
                        break;
                    case 6:
                        instrumentList = instrumentList.Where(x => x.State == "درحال انتظار").ToList();
                        break;
                    case 1:
                        instrumentList = instrumentList.Where(x => x.State == "لغو شده").ToList();
                        break;
                    case 2:
                        instrumentList = instrumentList.Where(x => x.State == "سفارش به طور کامل اجرا شده است").ToList();
                        break;
                    case 3:
                        instrumentList = instrumentList.Where(x => x.State == "خطای هسته معاملات").ToList();
                        break;
                    case 7:
                        instrumentList = instrumentList.Where(x => x.State == "در صف").ToList();
                        break;
                    case 8:
                        instrumentList = instrumentList.Where(x => x.State == "در صف در انتظار تایید لغو").ToList();
                        break;
                    case 9:
                        instrumentList = instrumentList.Where(x => x.State == "در صف در انتظار تایید ویرایش").ToList();
                        break;
                    case 10:
                        instrumentList = instrumentList.Where(x => x.State == "قسمتی انجام شده").ToList();
                        break;
                    case 11:
                        instrumentList = instrumentList.Where(x => x.State == "رد شده").ToList();
                        break;
                }
            }
            var report = new AllOrderReport()
            {
                Model = instrumentList.Count == 0 ? new List<AllOrder>() : instrumentList.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = instrumentList.Count,
                OrderType = filter.OrderType,
            };
            return report;
        }

    }
}