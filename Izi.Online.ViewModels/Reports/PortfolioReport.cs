using Izi.Online.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public class PortfolioReport:Report<Asset>
    {
        public PortfolioSortColumn PortfolioSortColumn { get; set; } 
    }
}
