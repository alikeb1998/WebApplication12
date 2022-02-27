using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Order
{
    public class AllOrder 
    {
        public string isr { get; set; }
        public string state { get; set; }
        public string errorCode { get; set; }
        public int id { get; set; }
        public long quantity { get; set; }
        public long price  { get; set; }
        public DateTime CreatedAt { get; set; }
        public long ExecutedQ { get; set; }
        public int ValidityType { get; set; }
        public Instrument Instrument { get; set; }
    }
}
