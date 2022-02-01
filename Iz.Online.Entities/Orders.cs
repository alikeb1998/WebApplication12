using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class Orders
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int InstrumentId { get; set; }
        public int OrderSide { get; set; }
        public int OrderType { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public int ValidityType { get; set; }
        public DateTime ValidityDate { get; set; }
        public int DisclosedQuantity { get; set; }
        public DateTime RegisterOrderDate { get; set; }
        public long OrderId { get; set; }
        public string Isr { get; set; }
        public DateTime CreateOrderDate { get; set; }
        public DateTime OmsResponseDate { get; set; }
        public long OmsQty { get; set; }
        public long OmsPrice { get; set; }
        public int StatusCode { get; set; }
        

    }
}
