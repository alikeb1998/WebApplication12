using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;


namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        
        bool UpdateInstrumentList();
        ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits(SelectedInstrument model);
        ResultModel<InstrumentPriceDetails>   Price(SelectInstrumentDetails model);
        ResultModel<Details> Details(SelectInstrumentDetails model);
       // InstrumentStates States(Instrument model);
    }
}
