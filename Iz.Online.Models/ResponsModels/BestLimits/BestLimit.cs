using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.BestLimits
{
    public class BestLimit
    {
        public OrderRow orderRow1 { get; set; }
        public OrderRow orderRow2 { get; set; }
        public OrderRow orderRow3 { get; set; }
        public OrderRow orderRow4 { get; set; }
        public OrderRow orderRow5 { get; set; }
        public OrderRow orderRow6 { get; set; }
        public bool changeRow1 { get; set; }
        public bool changeRow2 { get; set; }
        public bool changeRow3 { get; set; }
        public bool changeRow4 { get; set; }
        public bool changeRow5 { get; set; }
        public bool changeRow6 { get; set; }
    }
}
