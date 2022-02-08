using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class AdministrationController : BaseApiController
    {
        #region ctor

        public IInstrumentsService _instrumentsService { get; set; }


        public AdministrationController(IInstrumentsService instrumentsService)
        {
            _instrumentsService = instrumentsService;
        }

        #endregion

      //test push 
        
        //public string Instruments()
        //{
        //    _instrumentsService.
        //}
    }
}
