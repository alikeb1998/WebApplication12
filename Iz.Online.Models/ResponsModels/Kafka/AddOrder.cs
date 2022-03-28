using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class AddOrder
    {
       
            public string Customer { get; set; }
            public string Instrument { get; set; }
            public int ExecutedQuantity { get; set; }
            public int TradedPrice { get; set; }
            public int TradedState { get; set; }
            public DateTime TradedAt { get; set; }
            public int OrderSide { get; set; }
            public int OrderId { get; set; }

        
    }
}
