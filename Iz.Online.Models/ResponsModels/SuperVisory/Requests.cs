using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.SuperVisory
{
    public class Requests : BoResponseBaseModel
    {
        public List<Request> Data { get; set; }
    }
}
