using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using AddOrderResult = Iz.Online.OmsModels.ResponsModels.Order.AddOrderResult;
using UpdateOrder = Iz.Online.OmsModels.InputModels.Order.UpdateOrder;
using UpdatedOrder = Iz.Online.OmsModels.ResponsModels.Order.UpdatedOrder;
using CanceledOrder = Iz.Online.OmsModels.ResponsModels.Order.CanceledOrder;
using CancelOrder = Iz.Online.OmsModels.InputModels.Order.CancelOrder;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        string Token { get; set; }
        Task<ResultModel<AddOrderResult>> Add(AddOrderModel addOrderModel);
        Task<ResultModel<AllOrders>> GetAll();
        Task<ResultModel<ActiveOrdersResult>> GetAllActives();
        Task<ResultModel<UpdatedOrder>> Update(UpdateOrder model);
        Task<ResultModel<CanceledOrder>> Cancel(CancelOrder model);
    }
}
