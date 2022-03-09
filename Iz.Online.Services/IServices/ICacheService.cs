using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.IServices
{
   public interface ICacheService
    {
        InstrumentList InstrumentData(int instrumentId);
        Izi.Online.ViewModels.AppConfigs ConfigData(string key);
        List<InstrumentList> InstrumentData();
        List<Izi.Online.ViewModels.AppConfigs> ConfigData();
        int GetLocalInstrumentIdFromOmsId(int omsId);
    }
}
