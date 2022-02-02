﻿using Iz.Online.OmsModels.ResponsModels.Order;

namespace Izi.Online.ViewModels.Orders;

public class ActiveOrder
{
     public string InstrumentName { get; set; }
     public int OrderSide { get; set; }
     public long Quantity { get; set; }
     public long ExecutedQ { get; set; }
     public long RemainedQ { get; set; }
     public long OrderQtyWait { get; set; }
     public int ValidityType { get; set; }
     public ValidityInfo ValidityInfo { get; set; }
     public DateTime CreatedAt { get; set; }
     public string State { get; set; }
     public int ExecutePercent { get; set; }
}