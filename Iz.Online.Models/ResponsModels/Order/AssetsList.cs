using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.Order
{
    public class AssetsList:OmsResponseBaseModel
    {
        public List<Assets> Assets { get; set; }
    }
}
