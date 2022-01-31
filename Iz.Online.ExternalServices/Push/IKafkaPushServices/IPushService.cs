using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.Instruments;


namespace Iz.Online.ExternalServices.Push.IKafkaPushServices
{
    public interface IPushService
    {
        Task<List<InstrumentsDetails>> OnRefreshInstrumentDetails();
    }
}
