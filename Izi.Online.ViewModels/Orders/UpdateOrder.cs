using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Orders
{
    public class UpdateOrder
    {
        public int InstrumentId { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public int ValidityType { get; set; }
        public DateTime ValidityDate { get; set; }

    }
}
