using Iz.Online.ExternalServices.Rest.ExternalService;
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
        IExternalInstrumentService _externalInstrumentsServices { get; }
        IExternalOrderService _externalOrderService { get; }
        Task<ResultModel<List<WatchList>>> UserWatchLists(string customerId);
        Task<ResultModel<WatchListDetails>> WatchListDetails(SearchWatchList model);
        Task<ResultModel<List<WatchList>>> DeleteWatchList(SearchWatchList model);
        Task<ResultModel<WatchListDetails>> NewWatchList(NewWatchList model);
        Task<ResultModel<WatchListDetails>> AddInstrumentToWatchList(EditEathListItems model);
        Task<ResultModel<WatchListDetails>> RemoveInstrumentFromWatchList(EditEathListItems model);
        Task<ResultModel<List<WatchList>>> InstrumentWatchLists(InstrumentWatchLists model);
        Task<ResultModel<WatchListDetails>> UpdateWatchList(EditWatchList model);

    }
}
