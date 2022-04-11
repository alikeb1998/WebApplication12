using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.InputModels.SuperVisory
{
    public class GetAllnput
    {
        public string SessionID { get; set; }
        public DateTime CreateAtFrom { get; set; }
        public DateTime CreateAtTo { get; set; }
        public long InstrumentId { get; set; }
        public List<int> Status { get; set; }



    }
}
