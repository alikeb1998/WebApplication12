﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments.BestLimit
{
    public class OrderRow
    {
        public string volumeBestBuy { get; set; }
        public string countBestBuy { get; set; }
        public string priceBestBuy { get; set; }
        public string priceBestSale { get; set; }
        public string countBestSale { get; set; }
        public string volumeBestSale { get; set; }
    }
}