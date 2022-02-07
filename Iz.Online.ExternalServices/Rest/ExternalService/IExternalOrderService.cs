using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using AddOrderResult = Iz.Online.OmsModels.ResponsModels.Order.AddOrderResult;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        AddOrderResult Add(AddOrderModel addOrderModel);
        AllOrders GetAll(OmsBaseModel getAllModel);
        ActiveOrdersResult GetAllActives(ViewBaseModel baseModel);
       // AssetsList GetAllAssets(ViewBaseModel baseModel);

    }
}
