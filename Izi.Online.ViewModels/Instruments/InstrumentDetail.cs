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
        public string instrumentId { get; set; }
        public string yesterdayPrice { get; set; }
        public string closingPrice { get; set; }
        public string lastPrice { get; set; }
        public string firstPrice { get; set; }
        public string lowPrice { get; set; }
        public string highPrice { get; set; }
        public int numberOfTrades { get; set; }
        public int volumeOfTrades { get; set; }
        public string valueOfTrades { get; set; }
        public string lastTradeDate { get; set; }

         public string BidPrice { get; set; }
        public string AskPrice { get; set; }
        public float ChangePercent { get; set; }




    }
}
