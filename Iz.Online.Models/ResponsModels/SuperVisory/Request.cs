using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.SuperVisory
{
    public  class Request
    {
        public long RequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; }
        public string InstrumentName { get; set; }
        public bool HasStockSheet { get; set; }
        public int LastStatus { get; set; }
        public string LastDescription { get; set; }
    }
}

