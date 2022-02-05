using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instrument = Izi.Online.ViewModels.Instruments.Instruments;

namespace Izi.Online.ViewModels.Orders
{
    public class Asset
    {
        public string Name { get; set; }
        public long TradeableQuantity { get; set; }
        public long LastPrice { get; set; }
        public long AvgPrice { get; set; }
        public long FianlAmount { get; set; }
        public long Gav { get; set; }
        public long ProfitAmount { get; set; }
        public long ProfitPercent { get; set; }
        public long SellProfit { get; set; }


    }
}
