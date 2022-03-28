using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.SignalR
{
    public  class InstrumentDetail
    {
        public int State { get; set; }
        public string StateText { get; set; }
        public int GroupState { get; set; }
        public string GroupStateText { get; set; }
        public string PriceMax { get; set; }
        public string PriceMin { get; set; }
        public string NscCode { get; set; }
        public string yesterdayPrice { get; set; }
        public string closingPrice { get; set; }
        public string lastPrice { get; set; }
        public string firstPrice { get; set; }
        public string lowPrice { get; set; }
        public string highPrice { get; set; }
        public string numberOfTrades { get; set; }
        public string volumeOfTrades { get; set; }
        public string valueOfTrades { get; set; }
        public string lastTradeDate { get; set; }

        public double BidPrice { get; set; }
        public double AskPrice { get; set; }
        public float LastPriceChangePercent { get; set; }
        public long Tick { get; set; }
        public double BuyCommissionRate { get; set; }
        public double SellCommissionRate { get; set; }
    }
}
