using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class InstrumentComment
    {
        public string Id { get; set; }
        public string CommentText { get; set; }
        
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }

        public Instrument Instrument { get; set; }
        public long InstrumentId { get; set; }
    }
}
