
using entity = Iz.Online.Entities;
namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class InstrumentGroup
    {
        //
        public static implicit operator entity.InstrumentBourse(InstrumentGroup model)
        {
            return new entity.InstrumentBourse() { BourseId = model.id, Code = model.code, Name = model.name, borse = model.borse };
        }
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int borse { get; set; }

    }
}
