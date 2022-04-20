using Iz.Online.OmsModels.ResponsModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.SignalR
{
    public class Order
    {
        public string instrumentName { get; set; }
        public int orderSide { get; set; }
        public string orderSideText { get; set; }
        public long quantity { get; set; }
        public long executedQ { get; set; }
        public long remainedQ { get; set; }
        public long price { get; set; }
        public long orderQtyWait { get; set; }
        public int validityType { get; set; }
        public ValidityInfo validityInfo { get; set; }
        public DateTime createdAt { get; set; }
        public string state { get; set; }
        public string stateText { get; set; }
        public double executePercent { get; set; }
        public int instrumentId { get; set; }
        public string nscCode { get; set; }
        public long orderId { get; set; }

      
    }
}
