﻿
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Izi.Online.ViewModels.Instruments.SelectInstrumentDetails;

namespace Iz.Online.Services.IServices
{
    public interface IInstrumentsService
    {
        IExternalInstrumentService _externalInstrumentsService { get;  }
        ResultModel<List<Instruments>> Instruments();
        ResultModel<List<InstrumentList>> InstrumentList();
        ResultModel<InstrumentDetail> Detail(SelectInstrumentDetails model);
        ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model);
        ResultModel<string> GetInstrumentComment(GetInstrumentComment model);
        
        
    }
}
