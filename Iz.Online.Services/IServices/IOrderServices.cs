using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.IServices
{
    public interface IOrderServices
    {
        AddOrderResult Add(AddOrderModel addOrderModel);
        
        OrdersList AllActive(ViewBaseModel addOrderModel);
        OrdersList All(ViewBaseModel addOrderModel);
    }
}
