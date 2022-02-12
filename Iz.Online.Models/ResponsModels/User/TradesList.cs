using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class TradesList:OmsResponseBaseModel
    {
        public List<Trade> Trades{get;set;}
    }
}
