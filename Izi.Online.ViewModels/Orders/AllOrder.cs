using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Orders
{
    public class AllOrder
    {
        public DateTime CreatedAt { get; set; }
        public string InstrumentName { get; set; }
        public int OrderSide { get;set; }
        public long Quantity { get; set; }
        public long price { get;set; }  
        public long Id { get; set; }    
        public int ValidityType { get;set;} 
        public string ValidityTypeText { get; set; }
        public long ExecutedQ { get; set; }
        public long ValueOfExeCutedQ { get; set; }  
        public string State { get; set; }
        public int InstrumentId { get; set; }
    }
}
