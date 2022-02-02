using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;

namespace Iz.Online.Services.IServices
{
    public interface IOrderServices
    {
        AddOrderResult Add(AddOrderModel addOrderModel);
        List<ActiveOrder> AllActive(ViewBaseModel viewBaseMode);
        //List<OmsModels.ResponsModels.Order.AddOrderResult> All(GetAll getAllModel);
    }
}
