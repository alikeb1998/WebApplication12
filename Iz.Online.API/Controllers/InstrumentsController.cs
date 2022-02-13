using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.Extensions.Caching.Distributed;




namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]


    /// <summary>
    ///نام دیده بان
    /// </summary>
    public class InstrumentsController : BaseApiController
    {
        #region ctor

        private readonly IInstrumentsService _instrumentsService;
        private readonly IExternalInstrumentService _externalInstrumentService;
        private readonly IDistributedCache _redis;

        public InstrumentsController(IInstrumentsService instrumentsService, IExternalInstrumentService externalInstrumentService, IDistributedCache redis)
        {
            _instrumentsService = instrumentsService;
            _externalInstrumentService = externalInstrumentService;
            _redis = redis;
        }

        #endregion

        //update instruments list.
        [HttpGet("UpdateInstrumentsDb")]
        public ResultModel<bool> UpdateInstrumentsDb()
        {
            
            var updateResult = _externalInstrumentService.UpdateInstrumentList();
            return new ResultModel<bool>(updateResult, updateResult);
        }

        //get instruments list.
        [HttpGet("List")]
        public ResultModel<List<InstrumentList>> List()
        {
            var instruments = _instrumentsService.InstrumentList();
            return instruments;
        }

        // get watchlist.
        [HttpGet("WatchLists")]
        public ResultModel<List<WatchList>> WatchLists(string customerId)
        {
            var result = _instrumentsService.UserWatchLists(customerId);
            return result;
        }

        //get watchlist details.
        [HttpPost("WatchListsDetails")]
        public ResultModel<WatchListDetails> WatchListsDetails([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.WatchListDetails(model);
            return result;
        }

        //delete watchlist
        [HttpPost("DeleteWatchList")]
        public ResultModel<List<WatchList>> DeleteWatchList([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.DeleteWatchList(model);
            return  result;
        }

        //add new watchlist.
        [HttpPost("NewWatchList")]
        public ResultModel<WatchListDetails> NewWatchList([FromBody] NewWatchList model)
        {
            var result = _instrumentsService.NewWatchList(model);
            return result;
        } 
        //add new watchlist.
        [HttpPost("UpdateWatchList")]
        public ResultModel<WatchListDetails> UpdateWatchList([FromBody] EditWatchList model)
        {
            var result = _instrumentsService.UpdateWatchList(model);
            return result;
        }

        //add an instrument to watchlists.
        [HttpPost("AddInstrumentToWatchList")]
        public ResultModel<WatchListDetails> AddInstrumentToWatchList([FromBody] EditEathListItems model)
        {
            var result = _instrumentsService.AddInstrumentToWatchList(model);
            return result;
        }
        //remove an instrument from watchlist.
        [HttpPost("RemoveInstrumentFromWatchList")]
        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList([FromBody] EditEathListItems model)
        {
            var result = _instrumentsService.RemoveInstrumentFromWatchList(model);
            return result;
        }

        [HttpPost("InstrumentWatchLists")]
        public ResultModel<List<WatchList>> InstrumentWatchLists([FromBody] InstrumentWatchLists model)
        {
            var result = _instrumentsService.InstrumentWatchLists(model);
            return result;
        }

        [HttpPost("BestLimits")]
        public ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits([FromBody] SelectedInstrument model)
        {
            var result = _externalInstrumentService.BestLimits(model);
            return result;
        }

        //get instrument details as prices or states and so on.
        [HttpPost("Detail")]
        public ResultModel<InstrumentDetail> Detail([FromBody] SelectInstrumentDetails model)
        {
            var result = _instrumentsService.Detail(model);
            return result;
        }

    }
}
