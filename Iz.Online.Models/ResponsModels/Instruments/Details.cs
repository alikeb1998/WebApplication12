using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class Details
    {
        public int State { get; set; }
        public InstrumentGroup Group { get; set; }
        public long PriceMax { get; set; }
        public long PriceMin { get; set; }
        public long Tick { get; set; }


    }
}
