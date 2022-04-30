using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.SignalR
{
    public class Trade
    {
        public string name { get; set; }
        public int orderSide { get; set; }
        public string orderSideText { get; set; }
        public long executedQ { get; set; }
        public long price { get; set; }
        public int state { get; set; }
        public DateTime tradedAt { get; set; }
        public string nscCode { get; set; }
        public long instrumentId { get; set; }
        public double quantity { get; set; }
        public double tradeValue { get; set; }
        public long tradeId { get; set; }
        public int internalState { get; set; }
        public string stateText { get; set; }
    }
}
