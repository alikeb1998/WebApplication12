﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Trades
{
    public class Trade
    {
        public string Name { get; set; } 
        public int OrderSide { get; set; }
        public long ExecutedQ { get; set; }
        public long Price { get; set; }
        public int State { get; set; }
        public DateTime TradedAt { get; set; }
        public string NscCode { get; set; }
        public int InstrumentId { get; set; }
    }
}
