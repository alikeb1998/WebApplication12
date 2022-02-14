using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iz.Online.Entities
{
    [Table(name: "Instruments", Schema = "Symbols")]

    public class Instrument
    {
        [Key]
        public long Id { get; set; }
        public long InstrumentId { get; set; }
        public string SymbolName { get; set; }
        public string CompanyName { get; set; }
        public string Isin { get; set; }
        public string Code { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long BaseVolume { get; set; }
        public int? SubSectorId { get; set; }
        public InstrumentSubSector SubSector { get; set; }
        public int? SectorId { get; set; }
        public InstrumentSector Sector { get; set; }
        public int? BourseId { get; set; }
        public InstrumentBourse Bourse { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public ICollection<WatchListsInstruments> WatchListsInstruments { get; set; }
        public float BuyCommisionRate { get; set; } 
        public float SellCommisionRate { get; set; }
        public long Tick { get; set; }
    }
}
