using Iz.Online.OmsModels.ResponsModels.Order;

namespace Izi.Online.ViewModels.Orders;

public class ActiveOrder
{
     public string InstrumentName { get; set; }
     public int OrderSide { get; set; }
     public string OrderSideText { get; set; }
     public long Quantity { get; set; }
     public long ExecutedQ { get; set; }
     public long RemainedQ { get; set; }
     public long Price { get; set; }
     public long OrderQtyWait { get; set; }
     public int ValidityType { get; set; }
     public ValidityInfo ValidityInfo { get; set; }
     public DateTime CreatedAt { get; set; }
     public string State { get; set; }
     public string StateText { get; set; }
     public double ExecutePercent { get; set; }
     public int InstrumentId { get; set; }
     public string NscCode { get; set; }
}