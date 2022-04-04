using Iz.Online.OmsModels.ResponsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.ChangeBroker
{
    public class Request
    {
        public long RequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InstrumentName { get; set; }
        public bool HasStockSheet { get; set; }
        public string LastDescription { get; set; }
    }
}
