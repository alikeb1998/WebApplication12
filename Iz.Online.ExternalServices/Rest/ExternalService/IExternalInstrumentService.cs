using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        bool UpdateInstrumentList();
        BestLimits BestLimits(SelectedInstrument model);
        InstrumentPrice Price(SelectedInstrument model);
    }
}
