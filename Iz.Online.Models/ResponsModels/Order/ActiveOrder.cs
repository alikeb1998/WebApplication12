using Iz.Online.OmsModels.InputModels;

namespace Iz.Online.OmsModels.ResponsModels.Order;

public class ActiveOrder 
{
     public Instrument instrument { get; set; }
     public string InstrumentName { get; set; }
     public int orderSide { get; set; }
     public long Quantity { get; set; }
     public long ExecutedQ { get; set; }
     public long RemainedQ { get; set; }
     public long OrderQtyWait { get; set; }
     public int ValidityType { get; set; }
     public DateTime ValidityInfo { get; set; }
     public int State { get; set; }
     public DateTime CreatedAt { get; set; }
}