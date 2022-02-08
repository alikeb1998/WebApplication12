using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;



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

        [HttpPost("UpdateInstrumentsDb")]
        
        public ResultModel<bool> UpdateInstrumentsDb([FromBody] ViewBaseModel model)
        {
            var updateResult = _externalInstrumentService.UpdateInstrumentList(model);
            return new ResultModel<bool>(updateResult, updateResult);
        }

        [HttpPost("List")]  
        
        public ResultModel<List<InstrumentList>> List()
        {
            var instruments = _instrumentsService.InstrumentList();
            return new ResultModel<List<InstrumentList>>(instruments);
        }

        [HttpPost("WatchLists")]
        
        public ResultModel<List<WatchList>> WatchLists([FromBody] ViewBaseModel model)
        {
            var result = _instrumentsService.UserWatchLists(model );
            return new ResultModel<List<WatchList>>(result);
        }

        [HttpPost("WatchListsDetails")]
        
        public ResultModel<WatchListDetails> WatchListsDetails([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.WatchListDetails(model );
            return new ResultModel<WatchListDetails>(result);
        }

        [HttpPost("DeleteWatchList")]
        
        public ResultModel<List<WatchList>> DeleteWatchList([FromBody] SearchWatchList model)
        {
            var result = _instrumentsService.DeleteWatchList(model );
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
            var result = _instrumentsService.AddInstrumentToWatchList(model );
            return new ResultModel<WatchListDetails>(result); 
            
        }

        [HttpPost("RemoveInstrumentFromWatchList")]
        
        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList([FromBody] EditEathListItems model)
        {
            var result = _instrumentsService.RemoveInstrumentFromWatchList(model );
            return new ResultModel<WatchListDetails>(result);
            
        }

        [HttpPost("InstrumentWatchLists")]
        
        public ResultModel<List<WatchList>> InstrumentWatchLists([FromBody] InstrumentWatchLists model)
        {
            var result = _instrumentsService.InstrumentWatchLists(model );
            return new ResultModel<List<WatchList>>(result);
        }

        [HttpPost("BestLimits")]
        
        public ResultModel< Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits([FromBody] SelectedInstrument model)
        {
            var result = _externalInstrumentService.BestLimits(model );
            return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(result);
        }

        //[HttpPost("Price")]

        //public ResultModel<InstrumentPrice> Price([FromBody] SelectedInstrument model)
        //{
        //    //var result = _externalInstrumentService.Price(model);
        //    var result = _instrumentsService.price(model);
        //    return new ResultModel<InstrumentPrice>(result);
        //}

        [HttpPost("Detail")]

        public ResultModel<InstrumentDetail> Detail([FromBody] Instrument model)
        {
            var result = _instrumentsService.Detail(model);
            return new ResultModel<InstrumentDetail>(result);
        }

    }
}
