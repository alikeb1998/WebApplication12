using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.IO;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class InitialReleaseController : BaseApiController
    {
        #region ctor
        private readonly IInitialReleaseService _InitialReleaseService;

        public InitialReleaseController(IInitialReleaseService initialReleaseService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _InitialReleaseService = initialReleaseService;
            _InitialReleaseService._externalInitialReleaseService.Token =  _token_;

        }
        #endregion
        [HttpGet("OpenOffers")]
        public async Task<IActionResult> OpenOffers()
        {
            var result = await _InitialReleaseService.OpenOffers();
            return new Respond<List<Offers>>().ActionRespond(result);
        }

        [HttpGet("ActiveOrders")]
        public async Task<IActionResult> ActiveOrders()
        {
            var result = await _InitialReleaseService.ActiveOrders();
            return new Respond<List<ActiveOrder>>().ActionRespond(result);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _InitialReleaseService.Delete(Id);
            return new Respond<bool>().ActionRespond(result);
        }
    }
}
