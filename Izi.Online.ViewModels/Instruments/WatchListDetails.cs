using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class WatchListDetails
    {
        public WatchListDetails()
        {
            WatchList = new WatchList();
            Instruments = new List<Instruments>();
        }
        public WatchList WatchList { get; set; }
        public List<Instruments> Instruments { get; set; }
    }
}
