using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class InstrumentList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string NscCode { get; set; }
        public int Bourse { get; set; }
    
       
    }
}
