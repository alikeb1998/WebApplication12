
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iz.Online.Entities
{
    [Table(name: "InstrumentBourse", Schema = "Symbols")]
    public class InstrumentBourse
    {
        [Key]
        public int Id { get; set; }
        public int BourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int borse { get; set; }
    }
}
