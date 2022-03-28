using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class OrderChange
    {
        public string State { get; set; }
        public string OrderParentId { get; set; }
        public string Customer { get; set; }
        public string Instrument { get; set; }
        public string OrderSide { get; set; }
        public string OrderType { get; set; }
        public string ValidityType { get; set; }
        public string ValidityInfo { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string DisclosedQuantity { get; set; }
        public string RemainingQuantity { get; set; }
        public string ExecutedQuantity { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ChangedAt { get; set; }
        public string OrderId { get; set; }
    }
}
