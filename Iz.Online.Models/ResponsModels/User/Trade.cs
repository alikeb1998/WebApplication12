using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using model = Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class Trade
    {
         public model.Order Order { get; set; }
        public long Price { get; set; }
        public DateTime TradedAt { get; set; }
        public string InternalState { get; set; }





    }
}
