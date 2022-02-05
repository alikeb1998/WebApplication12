using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Order
{
   public class Assets
    {
        public Instrument Instrument { get; set; }
        public long TradeableQuantity { get; set; }
        public long LastPrice { get; set; }
        public long AvgPrice { get; set; }
        public long FinalAmount { get; set; }
        public long Gav { get; set; }
        public long ProfitAmount { get; set; }
        public long ProfitPercent { get; set; }
        public long SellProfit { get; set; }


    }
}
