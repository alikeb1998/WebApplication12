using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class PagingParam<T>
    {
        public int PageSize { get; set; } = 10;  //default page size
        public int CurrentPage { get; set; } = 1;

        public string SortField { get; set; } = null;
        public T Filter { get; set; }
        public SortDirection SortDir { get; set; }
        public enum SortDirection
        {
            Ascending = 0,   //default as ascending
            Decending = 1
        }
    }
}
