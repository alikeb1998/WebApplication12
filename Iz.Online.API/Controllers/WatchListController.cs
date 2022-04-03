﻿using Iz.Online.API.Infrastructure;
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
        private readonly IInstrumentsService _instrumentsService;

        public WatchListController(IWatchListsService watchListsServices, IHttpContextAccessor httpContextAccessor, IInstrumentsService instrumentsService) : base(httpContextAccessor)
        {
            _watchListsServices = watchListsServices;

            _watchListsServices._externalInstrumentsServices.Token = _watchListsServices._externalOrderService.Token = _token_;
        }
        #endregion
        // get watchlist.
      
        [HttpGet("WatchLists")]
        public IActionResult WatchLists(string TradingId)
        {
            var result = _watchListsServices.UserWatchLists(TradingId);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        //get watchlist details.
        [HttpPost("WatchListsDetails")]
        public IActionResult WatchListsDetails([FromBody] SearchWatchList model)
        {
            var result = _watchListsServices.WatchListDetails(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        //delete watchlist
        [HttpPost("DeleteWatchList")]
        public IActionResult DeleteWatchList([FromBody] SearchWatchList model)
        {
            var result = _watchListsServices.DeleteWatchList(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
        
        //add new watchlist.
        [HttpPost("NewWatchList")]
        public IActionResult NewWatchList([FromBody] NewWatchList model)
        {
            var result = _watchListsServices.NewWatchList(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
        //add new watchlist.
        [HttpPost("UpdateWatchList")]
        public IActionResult UpdateWatchList([FromBody] EditWatchList model)
        {
            var result = _watchListsServices.UpdateWatchList(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        //add an instrument to watchlists.
        [HttpPost("AddInstrumentToWatchList")]
        public IActionResult AddInstrumentToWatchList([FromBody] EditEathListItems model)
        {
            var result = _watchListsServices.AddInstrumentToWatchList(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
        //remove an instrument from watchlist.
        [HttpPost("RemoveInstrumentFromWatchList")]
        public IActionResult RemoveInstrumentFromWatchList([FromBody] EditEathListItems model)
        {
            var result = _watchListsServices.RemoveInstrumentFromWatchList(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
        //یک نماد در چکدام دیده بان ها است ؟
        [HttpPost("InstrumentWatchLists")]
        public IActionResult InstrumentWatchLists([FromBody] InstrumentWatchLists model)
        {
            var result = _watchListsServices.InstrumentWatchLists(model);
            return result.StatusCode switch
            {
                200 => Ok(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

      
    }
}
