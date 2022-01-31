using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class WalletDetails
    {
        public int withdrawable { get; set; }
        public int nonWithdrawable { get; set; }
        public int lendedCredit { get; set; }
        public int blockedValue { get; set; }
        public int buyingPower { get; set; }
    }
}
