using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.IO
{
    public class ActiveOrder
    {
        public int ExequtedPercent { get; set; }
        public DateTime BookDate { get; set; }
        public string Name { get; set; }    
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int State { get; set; }
        public string StateText { get; set; }

    }
}
