using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels
{
    public class BoResponseBaseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int HttpStatusCode { get; set; }
        public int StatusCode { get; set; }

    }
}
