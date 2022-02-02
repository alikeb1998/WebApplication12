using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        AddOrderResult Add(AddOrder addOrderModel, string token);
        AllOrders GetAll(OmsBaseModel getAllModel, string token);
        ActiveOrdersResult GetAllActives(ViewBaseModel baseModel, string token);

    }
}
