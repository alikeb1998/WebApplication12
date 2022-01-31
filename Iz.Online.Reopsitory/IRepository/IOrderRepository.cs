using Izi.Online.ViewModels.Orders;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IOrderRepository : IBaseRepository
    {
        bool Add(AddOrderModel addOrderModel);
    }
}
