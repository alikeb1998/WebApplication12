using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class CustomerInfo : OmsResponseBaseModel
    {
        public int id { get; set; }
        public string nameFirst { get; set; }
        public string nameLast { get; set; }
        public string borseCode { get; set; }
        public string nationalID { get; set; }
        public string tradingID { get; set; }
    }
}
