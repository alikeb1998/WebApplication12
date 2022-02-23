using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class WatchListController : BaseApiController
    {
        #region ctor
        public IWatchListsService _watchListsServices { get; set; }

        public WatchListController(IWatchListsService watchListsServices, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _watchListsServices = watchListsServices; 
           

        }
        #endregion
        // get watchlist.
        [HttpGet("WatchLists")]
        public ResultModel<List<WatchList>> WatchLists(string customerId)
        {
            var result = _watchListsServices.UserWatchLists(customerId);
            return result;
        }

        //get watchlist details.
        [HttpPost("WatchListsDetails")]
        public ResultModel<WatchListDetails> WatchListsDetails([FromBody] SearchWatchList model)
        {
            var result = _watchListsServices.WatchListDetails(model);
            return result;
        }

        //delete watchlist
        [HttpPost("DeleteWatchList")]
        public ResultModel<List<WatchList>> DeleteWatchList([FromBody] SearchWatchList model)
        {
            var result = _watchListsServices.DeleteWatchList(model);
            return result;
        }

        //add new watchlist.
        [HttpPost("NewWatchList")]
        public ResultModel<WatchListDetails> NewWatchList([FromBody] NewWatchList model)
        {
            var result = _watchListsServices.NewWatchList(model);
            return result;
        }
        //add new watchlist.
        [HttpPost("UpdateWatchList")]
        public ResultModel<WatchListDetails> UpdateWatchList([FromBody] EditWatchList model)
        {
            var result = _watchListsServices.UpdateWatchList(model);
            return result;
        }

        //add an instrument to watchlists.
        [HttpPost("AddInstrumentToWatchList")]
        public ResultModel<WatchListDetails> AddInstrumentToWatchList([FromBody] EditEathListItems model)
        {
            var result = _watchListsServices.AddInstrumentToWatchList(model);
            return result;
        }
        //remove an instrument from watchlist.
        [HttpPost("RemoveInstrumentFromWatchList")]
        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList([FromBody] EditEathListItems model)
        {
            var result = _watchListsServices.RemoveInstrumentFromWatchList(model);
            return result;
        }
        //یک نماد در چکدام دیده بان ها است ؟
        [HttpPost("InstrumentWatchLists")]
        public ResultModel<List<WatchList>> InstrumentWatchLists([FromBody] InstrumentWatchLists model)
        {
            var result = _watchListsServices.InstrumentWatchLists(model);
            return result;
        }

        // گذاشتن یادداشت برای  نماد
        [HttpPost("AddComment")]
        public ResultModel<bool> AddComment([FromBody] AddCommentForInstrument model)
        {
            var result = _watchListsServices.AddCommentToInstrument(model);
            return result;
        }
        // مشاهده یادداشت یک  نماد
        [HttpPost("GetComment")]
        public ResultModel<string> GetComment([FromBody] GetInstrumentComment model)
        {
            var result = _watchListsServices.GetInstrumentComment(model);
            var a = result.ToString();
            return result;
        }
    }
}
