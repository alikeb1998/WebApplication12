using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.Users.InputModels
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password{ get; set; }
        public string CaptchaId { get; set; }
        public string CaptchaCode { get; set; }
    }
}
