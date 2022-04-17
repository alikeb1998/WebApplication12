using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.News
{
    public class ObserverMessages:OmsResponseBaseModel
    {
        public List<ObserverMessage> Messages { get; set; }    
    }
}
