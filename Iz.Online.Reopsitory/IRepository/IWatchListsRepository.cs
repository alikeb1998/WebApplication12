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
        Task<ResultModel<List<WatchList>>> GetUserWatchLists(string customerId);
        Task<ResultModel<WatchListDetails>> GetWatchListDetails(SearchWatchList model);
        Task<ResultModel<List<WatchList>>> DeleteWatchList(SearchWatchList model);
        Task<ResultModel<WatchListDetails>> NewWatchList(NewWatchList model);
        Task<ResultModel<WatchListDetails>> AddInstrumentToWatchList(EditEathListItems model);
        Task<ResultModel<WatchListDetails>> RemoveInstrumentFromWatchList(EditEathListItems model);
        Task<ResultModel<List<WatchList>>> InstrumentWatchLists(InstrumentWatchLists model);
                                         
        Task<ResultModel<WatchListDetails>> UpdateWatchList(EditWatchList model);
        
    }
}
