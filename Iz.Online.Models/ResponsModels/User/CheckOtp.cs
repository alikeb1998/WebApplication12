using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class CheckOtp : OmsResponseBaseModel
    {
        public string Token { get; set; }
        public string SocketToken { get; set; }
    }
}
