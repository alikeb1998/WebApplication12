using entity = Iz.Online.Entities;

namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class Instrument
    {
        public static implicit operator entity.Instrument(Instrument model)
        {
            return new entity.Instrument()
            {
                BaseVolume = model.baseVolume,
                Code = model.code,
                CompanyName = model.text,
                Isin = model.isin,
                SymbolName = model.name,
                UpdatedAt = model.updatedAt,
                ProductType = model.product.typeCode1 + model.product.typeCode2,
                ProductCode = model.product.code,
                InstrumentId = model.Id,

                Bourse = model.group,
                Sector = model.sector,
                SubSector = model.subSector
            };
        }
        public int Id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string isin { get; set; }
        public InstrumentGroup group { get; set; }
        public InstrumentSector subSector { get; set; }
        public InstrumentSector sector { get; set; }
        public InstrumentProduct product { get; set; }
        public DateTime updatedAt { get; set; }
        public long baseVolume { get; set; }
        public long borse { get; set; }
    }
}
