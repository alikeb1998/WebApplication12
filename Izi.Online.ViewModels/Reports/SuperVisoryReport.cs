using Iz.Online.OmsModels.ResponsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class SuperVisoryReport
    {
        public string UserName { get; set; }
        public DateTime ChangeAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public long InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string Document { get; set; }
        public string DocumentName { get; set; }
        public string DocumentExtension { get; set; }
        public long RequestId { get; set; }
        public string StatusText { get; set; }

    }
}
