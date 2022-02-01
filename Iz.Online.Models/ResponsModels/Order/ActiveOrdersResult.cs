using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.OmsModels.ResponsModels.Order;

public class ActiveOrdersResult : OmsResponseBaseModel
{
     public List<ResponsModels.Order.Order> Orders { get; set; }
}