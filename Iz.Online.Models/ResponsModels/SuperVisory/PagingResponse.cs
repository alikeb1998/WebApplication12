using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.SuperVisory
{
   
        public class PagingResponse
        {
            public List<SuperVisoryReport> Items { get; set; }
            public MetaData MetaData { get; set; }
        }

        
    
}
