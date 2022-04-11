using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.ChangeBroker
{
    public class GetReq
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string Description { get; set; }
        public bool HasStockSheet { get; set; }
        public string Document { get; set; }
        public string DocumentsName { get; set; }
        public string DocumentExtension { get; set; }
    }
}
