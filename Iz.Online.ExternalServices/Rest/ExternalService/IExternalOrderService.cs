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
        public string token { get; set; }

        ResultModel<AddOrderResult> Add(AddOrderModel addOrderModel);
        ResultModel<AllOrders> GetAll();
        ResultModel <ActiveOrdersResult> GetAllActives();
        ResultModel<UpdatedOrder> Update(UpdateOrder model);
        ResultModel<CanceledOrder> Cancel(CancelOrder model);

       // AssetsList GetAllAssets(ViewBaseModel baseModel);

    }
}
