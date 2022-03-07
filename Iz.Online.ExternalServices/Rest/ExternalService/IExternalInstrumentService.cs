using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;


namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
         string Token { get; set; }

         bool UpdateInstrumentList();
        ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits(string NscCode, long InstrumentId);
        ResultModel<InstrumentPriceDetails>   Price(string NscCode);
        ResultModel<Details> Details(long InstrumentId);
       // InstrumentStates States(Instrument model);
       void StartConsume();
    }
}
