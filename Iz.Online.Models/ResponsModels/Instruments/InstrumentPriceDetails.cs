using Iz.Online.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class InstrumentPriceDetails
    {
        public string  instrumentId { get; set; }
        public double? yesterdayPrice { get; set; }
        public double? closingPrice { get; set; }
        public double? lastPrice { get; set; }
        public double? firstPrice { get; set; }
        public double? minimumPrice { get; set; }
        public double? maximumPrice { get; set; }
        public int? numberOfTrades { get; set; }
        public int? volumeOfTrades { get; set; }
        public double? valueOfTrades { get; set; }
        public string lastTradeDate { get; set; }
        public DateTime lastTradeDate2 => DateHelper.GetTimeFromString(lastTradeDate);
    }
}
