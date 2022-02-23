using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.Orders;

namespace Izi.Online.ViewModels.Reports
{
    public class OrderReport : Report<ActiveOrder>
    {
        public OrderSortColumn OrderSortColumn { get; set; }
    }
}
