
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.IServices
{
    public interface IInstrumentsService
    {
        List<Instruments> Instruments();
        List<WatchList> UserWatchLists(ViewBaseModel model);
        WatchListDetails WatchListDetails(SearchWatchList model);
        List<WatchList> DeleteWatchList(SearchWatchList model);
        WatchListDetails NewWatchList(NewWatchList model);
        WatchListDetails AddInstrumentToWatchList(EditEathListItems model);
        WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model);
        List<WatchList> InstrumentWatchLists(InstrumentWatchLists model);
        List<InstrumentList> InstrumentList();
    }
}
