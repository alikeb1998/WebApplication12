using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class CustomerWallet
    {
     
            public DateTime DateOfEvent { get; set; }
            public string Customer { get; set; }
            public long BuyingPower { get; set; }
            public long Withdrawable { get; set; }
            public int NonWithdrawable { get; set; }
            public int LendedCredit { get; set; }
            public int BlockedValue { get; set; }
        

    }
}
