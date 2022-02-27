
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Izi.Online.ViewModels.Instruments.SelectInstrumentDetails;

namespace Iz.Online.Services.IServices
{
    public interface IInstrumentsService
    {
        public string _token { get; set; }
        ResultModel<List<Instruments>> Instruments();
        ResultModel<List<InstrumentList>> InstrumentList();
        ResultModel<InstrumentDetail> Detail(SelectInstrumentDetails model);
        ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model);
        ResultModel<string> GetInstrumentComment(GetInstrumentComment model);
        
        
    }
}
