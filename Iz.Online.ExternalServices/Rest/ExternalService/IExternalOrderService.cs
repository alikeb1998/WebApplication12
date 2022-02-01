using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        AddOrderResult Add(AddOrder addOrderModel);
        AllOrders GetAll(OmsBaseModel getAllModel);
        ActiveOrdersResult GetAllActives(ViewBaseModel baseModel);

    }
}
