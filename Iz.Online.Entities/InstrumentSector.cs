
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iz.Online.Entities
{
    [Table(name: "InstrumentSectors", Schema = "Symbols")]
    public class InstrumentSector
    {
        [Key]
        public int Id { get; set; }
        public int SectorId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
