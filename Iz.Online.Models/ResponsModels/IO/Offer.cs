using Iz.Online.OmsModels.ResponsModels.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.IO
{
    public class Offer
    {
        public long Id { get; set; }
        public Instrument Instrument { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public long PriceMin { get; set; }
        public long PriceMax { get; set; }
        public long QuantityMin{get;set;}
        public long QuantityMax{get;set;}
        public DateTime OfferStartsAt { get;set;}
        public DateTime OfferClosesAt { get;set;}
        public DateTime CreatedAt { get;set;}
        public bool IsActive { get; set; }

    }
}
