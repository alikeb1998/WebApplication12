using Iz.Online.API.Infrastructure;
using Iz.Online.OmsModels.InputModels.SuperVisory;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ChangeBroker;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]

    public class ChangeBrokerController : BaseApiController
    {
        private readonly IChangeBrokerService _changeBrokerService;
        public ChangeBrokerController(IHttpContextAccessor httpContextAccessor, IChangeBrokerService brokerService) : base(httpContextAccessor)
        {
            _changeBrokerService = brokerService;
        }

        [HttpPost("AllRequests")]
        public async Task<IActionResult> AllRequests([FromBody]GetAllnput model)
        {
            var result = await _changeBrokerService.AllRequests(model);
            return new Respond<List<Request>>().ActionRespond(result);
        }
        [HttpPost("GetDoc")]
        public async Task<IActionResult> RequestDetails([FromBody] BaseInput model)
        {
            var result = await _changeBrokerService.GetDoc(model);
            return new Respond<DocDto>().ActionRespond(result);
        }

        [HttpPost("GetOne")]
        public async Task<IActionResult> GetOne([FromBody] BaseInput model)
        {
            var result = await _changeBrokerService.GetOne(model);
            return new Respond<GetReq>().ActionRespond(result);
        }

        [HttpPost("AddOne")]
        public async Task<IActionResult> AddRequest([FromBody] OmsModels.InputModels.SuperVisory.NewRequest model)
        {
            var result = await _changeBrokerService.AddRequest(model);
            return new Respond<bool>().ActionRespond(result);
        }

        [HttpPost("EditRequest")]
        public async Task<IActionResult> EditRequest([FromBody] EditModel model)
        {
            var result = await _changeBrokerService.EditRequest(model);
            return new Respond<bool>().ActionRespond(result);
        }

        [HttpPost("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest([FromBody] BaseInput model)
        {
            var result = await _changeBrokerService.DeleteRequest(model);
            return new Respond<bool>().ActionRespond(result);
        }


        [HttpPost("RequestHistory")]
        public async Task<IActionResult> RequestHistory([FromBody] BaseInput model)
        {
            var result = await _changeBrokerService.RequestHistory(model);
            return new Respond<List<RequestsHistory>>().ActionRespond(result);
        }
        [HttpPost("Report")]
        public async Task<IActionResult> Report([FromBody] PagingParam<SuperVisoryFilter> filter)
        {
            var result = await _changeBrokerService.Report(filter);
            return new Respond<List<SuperVisoryReport>>().ActionRespond(result);
        }

    }
}
