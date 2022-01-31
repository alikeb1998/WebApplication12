using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        AddOrderResult Add(AddOrder addOrderModel);

    }
}
