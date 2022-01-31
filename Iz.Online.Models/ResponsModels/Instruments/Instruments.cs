


namespace Iz.Online.OmsModels.ResponsModels.Instruments
{
    public class Instruments: OmsResponseBaseModel
    {
 
        public Instruments()
        {
            instruments = new List<Instrument>();
        }
        public List<Instrument> instruments { get; set; }
    }
}

