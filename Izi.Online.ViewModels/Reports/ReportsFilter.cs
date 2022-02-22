using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class ReportsFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Type OrderBy { get; set; }
        public OrderType OrderType { get; set; }
    }
}
