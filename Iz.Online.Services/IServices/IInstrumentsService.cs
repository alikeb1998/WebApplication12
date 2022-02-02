
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.IServices
{
    public interface IInstrumentsService
    {
        List<Instruments> Instruments(string token);
        List<WatchList> UserWatchLists(ViewBaseModel model, string token);
        WatchListDetails WatchListDetails(SearchWatchList model, string token);
        List<WatchList> DeleteWatchList(SearchWatchList model, string token);
        WatchListDetails NewWatchList(NewWatchList model, string token);
        WatchListDetails AddInstrumentToWatchList(EditEathListItems model, string token);
        WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model, string token);
        List<WatchList> InstrumentWatchLists(InstrumentWatchLists model, string token);
        List<InstrumentList> InstrumentList(string token);
    }
}
