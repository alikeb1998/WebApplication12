using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetails = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentDetails;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentPrice;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        public string token { get; set; }
        bool UpdateInstrumentList();
        ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits(SelectedInstrument model);
        ResultModel<InstrumentPriceDetails>   Price(SelectInstrumentDetails model);
        ResultModel<Details> Details(SelectInstrumentDetails model);
       // InstrumentStates States(Instrument model);
    }
}
