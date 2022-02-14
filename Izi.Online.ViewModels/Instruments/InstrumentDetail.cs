using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class InstrumentDetail
    {
        public int State { get; set; }
        public int GroupState{get;set;}
        public long PriceMax { get; set; }
        public long PriceMin { get; set; }
        public string NscCode { get; set; }
        public double yesterdayPrice { get; set; }
        public double closingPrice { get; set; }
        public double lastPrice { get; set; }
        public double firstPrice { get; set; }
        public double lowPrice { get; set; }
        public double highPrice { get; set; }
        public int numberOfTrades { get; set; }
        public int volumeOfTrades { get; set; }
        public double valueOfTrades { get; set; }
        public DateTime? lastTradeDate { get; set; }

         public double  BidPrice { get; set; }
        public double AskPrice { get; set; }
        public float ChangePercent { get; set; }




    }
}
