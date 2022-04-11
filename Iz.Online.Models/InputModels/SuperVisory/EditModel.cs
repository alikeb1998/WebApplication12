using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.InputModels.SuperVisory
{
    public class EditModel
    {
        public long RequestId { get; set; }
        public string SessionID { get; set; }
        public long InstrumentId { get; set; }
        public string Description { get; set; }
        public bool HasStockSheet { get; set; }
        public string Document { get; set; }
        public string DocumentsName { get; set; }
        public string DocumentExtension { get; set; }
    }
}
