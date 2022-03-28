using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class InstrumentDetail
    {
        public string yesterdayPrice { get; set; }
        public string closingPrice { get; set; }
        public string lastPrice { get; set; }
        public string firstPrice { get; set; }
        public string minimumPrice { get; set; }
        public string maximumPrice { get; set; }
        public string numberOfTrades { get; set; }
        public string volumeOfTrades { get; set; }
        public string valueOfTrades { get; set; }
        public string instrumentId { get; set; }
        public string lastTradeDate { get; set; }
    }
}
