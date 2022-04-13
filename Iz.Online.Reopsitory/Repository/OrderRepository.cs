using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Orders;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iz.Online.Reopsitory.Repository
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly OnlineBackendDbContext _db;
        public OrderRepository(OnlineBackendDbContext dataBase) : base(dataBase)
        {
            _db = dataBase;

        }
    
        public async void Add(Orders addOrderModel)
        {
            try
            {
                _db.Orders.Add(addOrderModel);
               //await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }


        public async void Update(Orders updateOrderModel)
        {
            try
            {

                _db.Orders.Update(updateOrderModel);

                await _db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async void Cancel(Orders updateOrderModel)
        {
            try
            {
                _db.Orders.Remove(updateOrderModel);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public void LogException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
