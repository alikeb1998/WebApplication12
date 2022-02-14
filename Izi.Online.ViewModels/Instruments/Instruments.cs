using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class Instruments
    {
        public long Id { get; set; }
        public long InstrumentId { get; set; }
        public string SymbolName { get; set; }
        public string CompanyName { get; set; }
        public string Isin { get; set; }
        public string Code { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long BaseVolume { get; set; }
        public string SubSector { get; set; }
        public string Sector { get; set; }
        public int Bourse { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public int Tick { get; set; }

        public double BidPrice { get; set; }
        public double AskPrice { get; set; }
        public double LastPrice { get; set; }
        public double ClosePrice { get; set; }
        public float ChangePercent { get; set; }
        

        
    }
}
