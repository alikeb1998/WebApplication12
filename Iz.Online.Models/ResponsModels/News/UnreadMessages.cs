using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.News
{
    public class UnreadMessages:OmsResponseBaseModel
    {
        public List<string> Ids { get; set; }
    }
}
