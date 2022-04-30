using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.IO
{
    public class Offers
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public long PriceMin { get; set; }
        public long PriceMax { get; set; }
        public long QuantityMin { get; set; }
        public long QuantityMax { get; set; }
        public bool IsActive { get; set; }
    }
}
