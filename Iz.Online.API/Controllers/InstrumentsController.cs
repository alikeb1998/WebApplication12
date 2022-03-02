using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Microsoft.AspNetCore.Mvc;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;


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
        private readonly IExternalInstrumentService _externalInstrumentService;


        public InstrumentsController(IInstrumentsService instrumentsService, IHttpContextAccessor httpContextAccessor, IExternalInstrumentService externalInstrumentService) : base(httpContextAccessor)
        {
            _instrumentsService = instrumentsService;
            _instrumentsService._externalInstrumentsService.Token = _token_;
            _externalInstrumentService = externalInstrumentService;
            //_externalInstrumentService.StartConsume();

        }

        #endregion

        [HttpGet]
        public void StartConsume()
        {
            _externalInstrumentService.StartConsume();

        }
        //update instruments list.
        [HttpGet("UpdateInstrumentsDb")]
        public ResultModel<bool> UpdateInstrumentsDb()
        {
            return null;

            //var updateResult = _externalInstrumentService.UpdateInstrumentList();
            //return new ResultModel<bool>(updateResult, updateResult);
        }

        //get instruments list.
        [HttpGet("List")]
        public ResultModel<List<InstrumentList>> List()
        {
            var instruments = _instrumentsService.InstrumentList();
            return instruments;
        }



        [HttpPost("BestLimits")]
        public ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits([FromBody] SelectedInstrument model)
        {
            //model.InstrumentId = "IRO1FOLD0001";
            var result = _instrumentsService.BestLimits(model);
            return result;
        }

        //get instrument details as prices or states and so on.
        [HttpPost("Detail")]
        public ResultModel<InstrumentDetail> Detail([FromBody] SelectInstrumentDetails model)
        {
            //model.InstrumentId = 658;
            //model.NscCode = "IRO1FOLD0001";
            var result = _instrumentsService.Detail(model);
            return result;
        }
        // گذاشتن یادداشت برای  نماد
        [HttpPost("AddComment")]
        public ResultModel<bool> AddComment([FromBody] AddCommentForInstrument model)
        {
            var result = _instrumentsService.AddCommentToInstrument(model);
            return result;
        }
        // مشاهده یادداشت یک  نماد
        [HttpPost("GetComment")]
        public ResultModel<string> GetComment([FromBody] GetInstrumentComment model)
        {
            var result = _instrumentsService.GetInstrumentComment(model);
            var a = result.ToString();
            return result;
        }
    }
}
