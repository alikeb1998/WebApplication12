using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.BestLimits
{
    public class OrderRow
    {
        public double volumeBestBuy { get; set; }
        public double countBestBuy { get; set; }
        public double priceBestBuy { get; set; }
        public double priceBestSale { get; set; }
        public double countBestSale { get; set; }
        public double volumeBestSale { get; set; }
    }
}
