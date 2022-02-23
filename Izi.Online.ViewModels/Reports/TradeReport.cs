using Izi.Online.ViewModels.Trades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class TradeReport:Report<Trade>
    {
        public TradeSortColumn TradeSortColumn { get; set; }    
    }
}
