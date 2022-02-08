using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentStates = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentStates;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        bool UpdateInstrumentList(ViewBaseModel model);
        Izi.Online.ViewModels.Instruments.BestLimit.BestLimits BestLimits(SelectedInstrument model);
        InstrumentPrice Price(SelectedInstrument model);
        InstrumentDetails Details(InstrumentDetails model);
        InstrumentStates States(Instrument model);
    }
}
