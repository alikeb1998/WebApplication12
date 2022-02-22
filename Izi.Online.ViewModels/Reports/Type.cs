using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class Type
    {
        public OrderSortColumn orderBy{ get; set; }
        public TradeSortColumn TradeBy{ get; set; }
        public PortfolioSortColumn AssetsBy{ get; set; }
      
    }
}
