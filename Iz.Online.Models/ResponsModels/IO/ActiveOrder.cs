using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.IO
{
    public class ActiveOrder
    {
        public int Id { get; set; }
        public Offer Offer { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Rid { get; set; }
        public string Inr { get; set; }
        public int Fee { get; set; }
        public int Tax { get; set; }
        public int Amount { get; set; }
        public int ExecutedQ { get; set; }
        public int RemainedQ { get; set; }
        public string State { get; set; }
    }
}
