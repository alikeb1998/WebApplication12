using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;

namespace Iz.Online.Services.IServices
{
    public interface IOrderServices
    {
        Izi.Online.ViewModels.Orders.AddOrderResult Add(AddOrderModel addOrderModel);
        ActiveOrdersResult AllActive(ViewBaseModel viewBaseModel);
        //List<OmsModels.ResponsModels.Order.AddOrderResult> All(GetAll getAllModel);
    }
}
