using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class WalletDetails
    {
        public long withdrawable { get; set; }
        public long nonWithdrawable { get; set; }
        public long lendedCredit { get; set; }
        public long blockedValue { get; set; }
        public long buyingPower { get; set; }
    }
}
