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

    }
}
