using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class SuperVisoryFilter
    {
        public DateTime CreatedAtFrom { get; set; }
        public DateTime CreatedAtTo { get; set; }
        public long InstrumentId { get; set; }
        public int State { get; set; }
    }
}
