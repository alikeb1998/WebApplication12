namespace Iz.Online.OmsModels.ResponsModels.Order;

public class GetAllResult : OmsResponseBaseModel
{
     public List<Order> orders { get; set; }
}