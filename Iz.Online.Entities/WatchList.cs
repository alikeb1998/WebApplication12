using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class WatchList
    {
        public string Id { get; set; }
        public string WatchListName { get; set; }
        public string CustomerId { get; set; }
        //public Customer Customer { get; set; }
        public ICollection<WatchListsInstruments> WatchListsInstruments { get; set; }


    }
}
