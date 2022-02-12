
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Izi.Online.ViewModels.Instruments.SelectInstrumentDetails;

namespace Iz.Online.Services.IServices
{
    public interface IInstrumentsService
    {
        ResultModel<List<Instruments>> Instruments();
        ResultModel<List<WatchList>> UserWatchLists(ViewBaseModel model);
        ResultModel<WatchListDetails> WatchListDetails(SearchWatchList model);
        ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model);
        ResultModel<WatchListDetails> NewWatchList(NewWatchList model);
        ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model);
        ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model);
        ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model);
        ResultModel<List<InstrumentList>> InstrumentList();
        ResultModel<InstrumentDetail> Detail(SelectInstrumentDetails model);
    }
}
