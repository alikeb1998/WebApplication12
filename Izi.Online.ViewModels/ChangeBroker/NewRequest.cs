using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Izi.Online.ViewModels.ChangeBroker
{
    public class NewRequest
    {
        public long RequestId { get; set; }
        public long InstrumentId { get; set; }
        public string Description { get; set; }
        public bool HasStockSheet { get; set; }
        public IFormFile Document { get; set; }
        public string DocumentsName { get; set; }
        public string DocumentExtension { get; set; }
    }
}
