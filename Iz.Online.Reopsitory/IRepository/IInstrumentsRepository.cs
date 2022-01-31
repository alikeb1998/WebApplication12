using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IInstrumentsRepository : IBaseRepository
    {
        List<Instruments> GetInstrumentsList();
        List<InstrumentBourse> GetInstrumentBourse();
        List<InstrumentSector> GetInstrumentSector();
        List<InstrumentSubSector> GetInstrumentSubSectors();

        void AddInstrumentBourse(InstrumentBourse model);
        void AddInstrumentSector(InstrumentSector model);
        void AddInstrumentSubSectors(InstrumentSubSector model);
        void AddInstrument(Instrument model);

        List<WatchList> GetUserWatchLists(ViewBaseModel model);
        WatchListDetails GetWatchListDetails(SearchWatchList model);
        List<WatchList> DeleteWatchList(SearchWatchList model);
        WatchListDetails NewWatchList(NewWatchList model);
        WatchListDetails AddInstrumentToWatchList(EditEathListItems model);
        WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model);
        List<WatchList> InstrumentWatchLists(InstrumentWatchLists model);


    }
}
