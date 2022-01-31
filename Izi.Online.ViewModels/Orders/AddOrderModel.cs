using oms = Iz.Online.OmsModels.InputModels.Order;
using db =Iz.Online.Entities ;
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Orders
{
    public class AddOrderModel : ViewBaseModel
    {
        public static implicit operator oms.AddOrder(AddOrderModel model)
        {
            return new oms.AddOrder() { Authorization = model.CustomerId };
        }
        public static implicit operator db.Orders(AddOrderModel model)
        {
            return new db.Orders() 
            {
                Id = Guid.NewGuid().ToString(),
                DisclosedQuantity = 1,
                InstrumentId = 1,
                OrderSide = 1,
                OrderType = 1,
                Price = 123,
                Quantity = 456,
                RegisterOrderDate = DateTime.Now,
                CustomerId = "testUserId",
                ValidityDate = DateTime.Now.AddDays(2),
                ValidityType = 2
            };
        }
        //public string HubId { get; set; }
        public long InstrumentId { get; set; }
        public int OrderSide { get; set; }
        public int OrderType { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public int ValidityType { get; set; }
        public DateTime ValidityDate { get; set; }
        public int DisclosedQuantity { get; set; }
    }
}
