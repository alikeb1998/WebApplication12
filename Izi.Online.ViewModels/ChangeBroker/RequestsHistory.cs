using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.ChangeBroker
{
    public class RequestsHistory
    {
        public string UserName { get; set; }
        public DateTime ChangeAt { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
