namespace Iz.Online.OmsModels.ResponsModels.Order;

public class ActiveOrders: OmsResponseBaseModel
{
     public List<ActiveOrder> Orders { get; set; }
}