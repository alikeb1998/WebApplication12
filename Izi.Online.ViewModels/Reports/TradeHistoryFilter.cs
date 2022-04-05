using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class TradeHistoryFilter:ReportsFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<long> InstrumentId { get; set; }
        public int OrderSide { get; set; }
        public int State { get; set; }
    }
}
