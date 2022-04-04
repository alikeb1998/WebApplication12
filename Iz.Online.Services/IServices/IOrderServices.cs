using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using CanceledOrder = Izi.Online.ViewModels.Orders.CanceledOrder;
using CancelOrder = Izi.Online.ViewModels.Orders.CancelOrder;
using UpdatedOrder = Izi.Online.ViewModels.Orders.UpdatedOrder;
using UpdateOrder = Izi.Online.ViewModels.Orders.UpdateOrder;
using AllOrder = Izi.Online.ViewModels.Orders.AllOrder;
using Iz.Online.ExternalServices.Rest.ExternalService;

namespace Iz.Online.Services.IServices
{
    public interface IOrderServices
    {
        IExternalOrderService _externalOrderService { get; }
        Task<ResultModel<AddOrderResult>> Add(AddOrderModel addOrderModel);
        Task<ResultModel<List<ActiveOrder>>> AllActive();
        Task<ResultModel<OrderReport>> AllActivePaged(OrderFilter filter);
        Task<ResultModel<AllOrderReport>> AllSortedOrder(AllOrderCustomFilter filter);
        Task<ResultModel<UpdatedOrder>> Update(UpdateOrder model);
        Task<ResultModel<CanceledOrder>> Cancel(CancelOrder model);

    }
}
