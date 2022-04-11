using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.SuperVisory
{
    public class GetOne
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
