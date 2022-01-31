using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class InstrumentPrice : OmsResponseBaseModel
    {
        public InstrumentPriceDetails price { get; set; }
    }
}
