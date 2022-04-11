using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.SuperVisory
{
    public class RequestHistory
    {
        public string UserName { get; set; }
        public DateTime ChangeAt { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
}
