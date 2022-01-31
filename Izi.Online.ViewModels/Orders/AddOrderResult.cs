using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Orders
{
    public class AddOrderResult
    {

        public bool IsSuccess { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
        
    }
}
