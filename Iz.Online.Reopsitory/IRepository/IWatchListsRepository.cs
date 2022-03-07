using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IWatchListsRepository : IBaseRepository
    {
        ResultModel<List<WatchList>> GetUserWatchLists(string customerId);
        ResultModel<WatchListDetails> GetWatchListDetails(SearchWatchList model);
        ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model);
        ResultModel<WatchListDetails> NewWatchList(NewWatchList model);
        ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model);
        ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model);
        ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model);
        
        ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model);
        
    }
}
