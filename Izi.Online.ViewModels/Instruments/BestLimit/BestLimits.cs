using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments.BestLimit
{
    public class BestLimits
    {
        public OrderRow orderRow1 { get; set; }
        public OrderRow orderRow2 { get; set; }
        public OrderRow orderRow3 { get; set; }
        public OrderRow orderRow4 { get; set; }
        public OrderRow orderRow5 { get; set; }
        public OrderRow orderRow6 { get; set; }
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
