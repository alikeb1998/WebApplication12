using entity = Iz.Online.Entities;

namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class InstrumentSector
    {
        //using entity = Iz.Online.Entities;
        public static implicit operator entity.InstrumentSector(InstrumentSector model)
        {
            return new entity.InstrumentSector() { SectorId = model.id, Code = model.code, Name = model.name };
        }
        public static implicit operator entity.InstrumentSubSector(InstrumentSector model)
        {
            return new entity.InstrumentSubSector() { SubSectorId = model.id, Code = model.code, Name = model.name };
        }
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
