using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Order
{
    public class OrderChangeTopic
    {
        public long OrderId { get; set; }
        public OrderState State { get; set; }
        public long? OrderParentId { get; set; }
        public string Customer { get; set; }
        public string Instrument { get; set; }
        public OrderSide OrderSide { get; set; }
        public OrderType OrderType { get; set; }
        public ValidityType ValidityType { get; set; }
        public string ValidityInfo { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public long? DisclosedQuantity { get; set; }
        public long RemainingQuantity { get; set; }
        public long ExecutedQuantity { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
