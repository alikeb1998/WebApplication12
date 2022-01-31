using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class WatchListsInstruments
    {
        [Key, Column(Order = 1)]
        public long InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
        
        [Key, Column(Order = 2)]
        public string WatchListId { get; set; }
        public WatchList WatchList { get; set; }

    }
}
