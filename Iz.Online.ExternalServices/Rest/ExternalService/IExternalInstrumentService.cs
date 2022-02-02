using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        bool UpdateInstrumentList(ViewBaseModel model);
        BestLimits BestLimits(SelectedInstrument model);
        InstrumentPrice Price(SelectedInstrument model);
        InstrumentDetails Details(InstrumentDetails model);
    }
}
