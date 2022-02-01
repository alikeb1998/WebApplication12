using Iz.Online.OmsModels.ResponsModels;

namespace Iz.Online.OmsModels.InputModels.Order;

public class GetAll: OmsResponseBaseModel
{
     public long Id { get; set; }
     public string Isr { get; set; }
}