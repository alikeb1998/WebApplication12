using oms = Iz.Online.OmsModels.InputModels.Order;
using db = Iz.Online.Entities;
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Orders
{
    public class AddOrderModel : ViewBaseModel
    {
        public static implicit operator oms.AddOrder(AddOrderModel model)
        {
            return new oms.AddOrder()
            {
                Authorization = model.Token,
                DisclosedQuantity = model.DisclosedQuantity,
                InstrumentId = model.InstrumentId,
                Price = model.Price,
                OrderSide = model.OrderSide,
                OrderType = model.OrderType,
                Quantity = model.Quantity,
                ValidityDate = model.ValidityDate,
                ValidityType = model.ValidityType

            };
        }
        public static implicit operator db.Orders(AddOrderModel model)
        {
            return new db.Orders()
            {
                Id = Guid.NewGuid().ToString(),
                DisclosedQuantity = model.DisclosedQuantity,
                InstrumentId = (int)model.InstrumentId,
                OrderSide = model.OrderSide,
                OrderType = model.OrderType,
                Price = model.Price,
                Quantity = model.Quantity,
                CustomerId = "testUserId",
                ValidityDate = DateTime.Now.AddDays(2),
                ValidityType = model.ValidityType
            };
        }

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
