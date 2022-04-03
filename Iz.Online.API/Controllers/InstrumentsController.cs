using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Instruments.BestLimit;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    //[JwtCustomAuthorize(role: "normalCustomer") ]

    /// <summary>
    ///نام دیده بان
    /// </summary>

    public class InstrumentsController : BaseApiController
    {
        #region ctor

        private readonly IInstrumentsService _instrumentsService;


        public InstrumentsController(IInstrumentsService instrumentsService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _instrumentsService = instrumentsService;
            _instrumentsService._externalInstrumentsService.Token = _instrumentsService._externalOrderService.Token = _token_;
            _instrumentsService.StartConsume();

        }

        #endregion


        [HttpGet("UpdateInstrumentsDb")]
        public ResultModel<bool> UpdateInstrumentsDb()
        {
            var result = _instrumentsService.UpdateInstrumentsDb();
            return new ResultModel<bool>(result, result);
        }

        //get instruments list.
        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _instrumentsService.InstrumentList();
           return new Respond<List<InstrumentList>>().ActionRespond(result);
            
        }



        [HttpPost("BestLimits")]
        public IActionResult BestLimits([FromBody]  SelectedInstrument model)
        {
            //model.InstrumentId = "IRO1FOLD0001";
            var result = _instrumentsService.BestLimits(model.InstrumentId , model.HubId);
            return new Respond<BestLimits>().ActionRespond(result);
        }

        //get instrument details as prices or states and so on.
        [HttpPost("Detail")]
        public IActionResult Detail([FromBody] SelectedInstrument model)
        {
            //model.InstrumentId = 658;
            //model.NscCode = "IRO1FOLD0001";
            var result = _instrumentsService.Detail(model.InstrumentId , model.HubId);
            return new Respond<InstrumentDetail>().ActionRespond(result);
        }
        // گذاشتن یادداشت برای  نماد
        [HttpPost("AddComment")]
        public IActionResult AddComment([FromBody] AddCommentForInstrument model)
        {
            var result = _instrumentsService.AddCommentToInstrument(model);
            return new Respond<bool>().ActionRespond(result);
        }
        // مشاهده یادداشت یک  نماد
        [HttpPost("GetComment")]
        public IActionResult GetComment([FromBody] GetInstrumentComment model)
        {
            var result = _instrumentsService.GetInstrumentComment(model);
            return new Respond<string>().ActionRespond(result);
        }
    }
}
