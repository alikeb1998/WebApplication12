using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class Portfolio
    {
        public string NationalId { get; set; }
        public string InstrumentCode { get; set; }
        public long Quantity { get; set; }
        public long LastPrice { get; set; }
        public long AveragePrice { get; set; }
        public long PriceOver { get; set; }
        public long AssetValue { get; set; }
        public long profitLoss { get; set; }
        public long ProfitLossPercent { get; set; }
        public long HeadToHeadPoint { get; set; }
    }
}
