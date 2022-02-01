using Iz.Online.OmsModels.ResponsModels;

namespace Izi.Online.ViewModels.Instruments;

public class InstrumentDetails
{
     public long maxPrice { get; set; }
     public long minPrice { get; set; }
     public long minAskPrice { get; set; }
     public long quantity { get; set; }
     public long lastPrice { get; set; }
     public long lastDayPrice { get; set; }
     public long firstPrice { get; set; }
     public long tradesCount { get; set; }
     public long tradesVolume { get; set; }
     public long tradesValue { get; set; }
     public string instrumentName { get; set; }
     public long realPrice { get; set; }
}