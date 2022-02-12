using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Iz.Online.OmsModels.InputModels.Order
{
    public class CancelOrder : OmsBaseModel 
    { 
        public int InstrumentId { get; set; }
    }
}
