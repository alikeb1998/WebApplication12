using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iz.Online.Entities
{
    [Table(name: "InstrumentSubSectors", Schema = "Symbols")]
    public class InstrumentSubSector
    {
        [Key]
        public int Id { get; set; }
        public int SubSectorId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
