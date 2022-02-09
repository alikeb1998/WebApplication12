using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Users
{
    public class Wallet
    {
        public long Withdrawable { get; set; }
        public long NonWithdrawable { get; set; }
        public long LendedCredit { get; set; }
        public long BlockedValue { get; set; }
        public long BuyingPower { get; set; }
    }
}
