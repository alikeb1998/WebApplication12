using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class OtpResult:OmsResponseBaseModel
    {
        public string OtpId { get; set; }
        public int ExpireAt { get; set; }
    }
}
