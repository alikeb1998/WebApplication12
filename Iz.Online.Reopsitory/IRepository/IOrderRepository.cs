using Iz.Online.Entities;
using Izi.Online.ViewModels.Orders;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IOrderRepository : IBaseRepository
    {
        void Add(Orders addOrderModel);
        void Update(Orders UpdateOrderModel);
        void Cancel(Orders UpdateOrderModel);
    }
}
