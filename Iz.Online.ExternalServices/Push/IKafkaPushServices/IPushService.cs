﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Instruments;


namespace Iz.Online.ExternalServices.Push.IKafkaPushServices
{
    public interface IPushService
    {
       Task  ConsumeRefreshInstrumentBestLimit(string InstrumentId);
       Task PushOrderAdded(List<string> CustomerHubsId, Izi.Online.ViewModels.Orders.ActiveOrder model);
    }
}
