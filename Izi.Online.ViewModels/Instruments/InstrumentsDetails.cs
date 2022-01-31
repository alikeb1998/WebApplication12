
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Instruments
{
    public class InstrumentsDetails
    {
        public long LastPrice { get; set; }
        public long ThresholdPriceMin { get; set; }
        public long ThresholdPriceMax { get; set; }
        public long MinExchangePrice { get; set; }
        public long MaxExchangePrice { get; set; }
        public long LastDayClosePrice { get; set; }
        public long FirstPrice { get; set; }
        public long TradeQuantity { get; set; }
        public long TradeValome { get; set; }
        public long TradeValue { get; set; }
        public long LastTradeTime { get; set; }
        public long LastTradePrice { get; set; }
        public long LastTradeChangePercent { get; set; }
        public OrderSide Side { get; set; }

    }
}
