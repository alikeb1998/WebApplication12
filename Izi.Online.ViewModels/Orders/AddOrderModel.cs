using oms = Iz.Online.OmsModels.InputModels.Order;
using db = Iz.Online.Entities;
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Orders
{
    public class AddOrderModel : ViewBaseModel
    {

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
