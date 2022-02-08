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
        bool UpdateInstrumentList(ViewBaseModel model);
        BestLimits BestLimits(SelectedInstrument model);
        InstrumentPrice Price(SelectedInstrument model);
        InstrumentDetails Details(InstrumentDetails model);
        InstrumentStates States(Instrument model);
    }
}
