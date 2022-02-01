using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.OmsModels.ResponsModels.Order;

public class ActiveOrdersResult : OmsResponseBaseModel
{
     public List<Order> Orders { get; set; }
}