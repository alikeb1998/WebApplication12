namespace Iz.Online.OmsModels.InputModels.Order;

public class ActiveOrderse : OmsBaseModel
{
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