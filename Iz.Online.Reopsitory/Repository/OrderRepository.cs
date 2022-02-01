using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Orders;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iz.Online.Reopsitory.Repository
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private OnlineBackendDbContext _db;
        public OrderRepository(OnlineBackendDbContext dataBase) : base(dataBase)
        {
            _db = dataBase;

        }
    
        public void Add(Orders addOrderModel)
        {
            try
            {

                _db.Orders.Add(addOrderModel);

                _db.SaveChanges();
                //return true;

            }
            catch (Exception e)
            {
               
                
                //return false;

                //Log

            }

        }


        public void LogException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
