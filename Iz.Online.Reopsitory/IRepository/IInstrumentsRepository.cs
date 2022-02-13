﻿using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IInstrumentsRepository : IBaseRepository
    {
        ResultModel<List<Instruments>> GetInstrumentsList();
        ResultModel<List<InstrumentBourse>> GetInstrumentBourse();
        ResultModel<List<InstrumentSector>> GetInstrumentSector();
        ResultModel<List<InstrumentSubSector>> GetInstrumentSubSectors();
        ResultModel<bool> AddInstrumentBourse(InstrumentBourse model);
        ResultModel<bool> AddInstrumentSector(InstrumentSector model);
        ResultModel<bool> AddInstrumentSubSectors(InstrumentSubSector model);
        ResultModel<bool> AddInstrument(Instrument model, int sectorId, int subSectorId, int bourseId);
        ResultModel<bool> UpdateInstruments(Instrument model, int sectorId, int subSectorId, int bourseId, long tick);
        ResultModel<List<WatchList>> GetUserWatchLists(string customerId);
        ResultModel<WatchListDetails> GetWatchListDetails(SearchWatchList model);
        ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model);
        ResultModel<WatchListDetails> NewWatchList(NewWatchList model);
        ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model);
        ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model);
        ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model);
        AppConfigs GetAppConfigs(string key);

        ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model);
    }
}
