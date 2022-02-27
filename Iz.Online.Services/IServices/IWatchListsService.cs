using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.IServices
{
    public interface IWatchListsService
    {
        ResultModel<List<WatchList>> UserWatchLists(string customerId);
        ResultModel<WatchListDetails> WatchListDetails(SearchWatchList model);
        ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model);
        ResultModel<WatchListDetails> NewWatchList(NewWatchList model);
        ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model);
        ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model);
        ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model);
        ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model);
  
    }
}
