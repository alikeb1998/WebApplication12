using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class Portfolio
    {
        public DateTime DateOfEvent { get; set; }
        public string InstrumentCode { get; set; }
        public string NationalId { get; set; }
        public int Quantity { get; set; }
        public int LastPrice { get; set; }
        public int AveragePrice { get; set; }
        public int PriceOver { get; set; }
        public int AssetValue { get; set; }
        public int ProfitLoss { get; set; }
        public int ProfitLossPercent { get; set; }
        public int HeadToHeadPoint { get; set; }
    }
}
