using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalOrderService
    {
        AddOrderResult Add(AddOrder addOrderModel);
        GetAllResult GetAll(GetAll getAllModel);
        ActiveOrdersResult GetAllActives(ViewBaseModel baseModel);

    }
}
