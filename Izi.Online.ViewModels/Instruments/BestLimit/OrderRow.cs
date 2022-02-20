using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments.BestLimit
{
    public class OrderRow
    {
        public double volumeBestBuy { get; set; }
        public double countBestBuy { get; set; }
        public double priceBestBuy { get; set; }
        public double priceBestSale { get; set; }
        public double countBestSale { get; set; }
        public double volumeBestSale { get; set; }
        public bool HasOrderBuy { get; set; }
        public bool HasOrderSell { get; set; }
        public double QtyOrderBuy { get; set; }
        public double QtyOrderSell { get; set; }
    }
}
