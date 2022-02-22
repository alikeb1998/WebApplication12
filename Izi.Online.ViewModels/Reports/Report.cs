using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Izi.Online.ViewModels.Reports
{
    public class Report<T>
    {
 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public Type Type { get; set; }
        public OrderType OrderType { get; set; }
        public List<T> Model { get; set; }
    }
}
