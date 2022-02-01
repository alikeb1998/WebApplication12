using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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

        [HttpGet("UpdateInstrumentsDb")]
        public ResultModel<bool> UpdateInstrumentsDb()
        {
            var updateResult = _externalInstrumentService.UpdateInstrumentList();
            return new ResultModel<bool>(updateResult, updateResult);
        }

        [HttpGet("List")]
        public ResultModel<List<InstrumentList>> List()
        {
            
            var instruments = _instrumentsService.InstrumentList();
            return new ResultModel<List<InstrumentList>>(instruments);
            
        }

        [HttpGet("WatchLists")]
        public ResultModel<List<WatchList>> WatchLists([FromBody] ViewBaseModel model)
        {
            var result = _instrumentsService.UserWatchLists(model);
            return new ResultModel<List<WatchList>>(result);
        }

        [HttpGet("WatchListsDetails")]
        public ResultModel<WatchListDetails> WatchListsDetails([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.WatchListDetails(model);
            return new ResultModel<WatchListDetails>(result);
            
        }

        [HttpPost("DeleteWatchList")]
        public ResultModel<List<WatchList>> DeleteWatchList([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.DeleteWatchList(model);
            return new ResultModel<List<WatchList>>(result);
        }

        [HttpPost("NewWatchList")]
        public ResultModel<WatchListDetails> NewWatchList([FromBody] NewWatchList model)
        {
            var result = _instrumentsService.NewWatchList(model);
            return new ResultModel<WatchListDetails>(result); 
        }

        [HttpPost("AddInstrumentToWatchList")]
        public ResultModel<WatchListDetails> AddInstrumentToWatchList([FromBody] EditEathListItems model)
        {
            var result = _instrumentsService.AddInstrumentToWatchList(model);
            return new ResultModel<WatchListDetails>(result); 
            
        }

        [HttpPost("RemoveInstrumentFromWatchList")]
        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList([FromBody] EditEathListItems model)
        {
            var result = _instrumentsService.RemoveInstrumentFromWatchList(model);
            return new ResultModel<WatchListDetails>(result);
            
        }

        [HttpGet("InstrumentWatchLists")]
        public ResultModel<List<WatchList>> InstrumentWatchLists([FromBody] InstrumentWatchLists model)
        {
            var result = _instrumentsService.InstrumentWatchLists(model);
            return new ResultModel<List<WatchList>>(result);
        }

        [HttpGet("BestLimits")]
        public ResultModel<BestLimits> BestLimits([FromBody] SelectedInstrument model)
        {
            var result = _externalInstrumentService.BestLimits(model);
            return new ResultModel<BestLimits>(result);
        }

        [HttpGet("Details")]
        public ResultModel<InstrumentDetails> Details([FromBody] SelectedInstrument model)
        {
            var detail = new InstrumentDetails()
            {
                quantity = 100,
                firstPrice = 1000,
                lastPrice = 1200,
                instrumentName = "سپهر",
                maxPrice = 1100,
                minPrice = 1000,
                realPrice = 1120,
                tradesCount = 120,
                tradesValue = 50000,
                tradesVolume = 300,
                lastDayPrice = 980,
                minAskPrice = 999
            };
            var result = _externalInstrumentService.Details(detail);
            return new ResultModel<InstrumentDetails>(result);
            
            // var result = _externalInstrumentService.Price(model);
            // return new ResultModel<InstrumentPrice>(result);
        }

    }
}
