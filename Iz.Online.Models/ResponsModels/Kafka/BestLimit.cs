using Iz.Online.OmsModels.ResponsModels.BestLimits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Kafka
{
    public class BestLimit
    {
        public OrderRow item1 { get; set; }
        public OrderRow item2 { get; set; }
        public OrderRow item3 { get; set; }
        public OrderRow item4 { get; set; }
        public OrderRow item5 { get; set; }
        public OrderRow item6 { get; set; }
        public bool IsSellQueue { get; set; }
        public bool IsBuyQueue { get; set; }
        public bool changeRow1 { get; set; }
        public bool changeRow2 { get; set; }
        public bool changeRow3 { get; set; }
        public bool changeRow4 { get; set; }
        public bool changeRow5 { get; set; }
        public bool changeRow6 { get; set; }
    }
}
