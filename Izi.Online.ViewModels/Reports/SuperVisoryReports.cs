using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.SuperVisory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class SuperVisoryReports : BoResponseBaseModel
    {
        public PagingResponse Data { get; set; }
    }
}
