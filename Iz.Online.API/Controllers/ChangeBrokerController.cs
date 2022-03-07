using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ChangeBroker;
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
       
        [HttpGet("AllRequests")]
        public ResultModel<List<Request>> AllRequests()
        {
            var updateResult = _changeBrokerService.AllRequests();
            return new ResultModel<List<Request>>(updateResult);
        } 
        [HttpPost("RequestDetails")]
        public ResultModel<Request> RequestDetails(long RequestId)
        {
            var updateResult = _changeBrokerService.RequestDetails(RequestId);
            return new ResultModel<Request>(updateResult);
        }

        [HttpPost("GetDoc")]
        public ActionResult GetDoc([FromBody] long DocumentId)
        {
            byte[] byteArray = _changeBrokerService.GetDocument(DocumentId);
            return new FileContentResult(byteArray, "application/octet-stream");

        }

        [HttpPost("AddRequest")]
        public ResultModel<long> AddRequest([FromBody] NewRequest model)
        {
            var result = _changeBrokerService.AddRequest(model);
            return new ResultModel<long>(result);
        }
      
        [HttpPost("EditRequest")]
        public ResultModel<bool> EditRequest([FromBody] NewRequest model)
        {
            var result = _changeBrokerService.EditRequest(model);
            return new ResultModel<bool>(result);
        }
      
        [HttpPost("DeleteRequest")]
        public ResultModel<bool> DeleteRequest([FromBody] long RequestId)
        {
            var result = _changeBrokerService.DeleteRequest(RequestId);
            return new ResultModel<bool>(result);
        }
      
        
        [HttpPost("RequestHistory")]
        public ResultModel<List<RequestsHistory>> RequestHistory([FromBody] long RequestId)
        {
            var result = _changeBrokerService.RequestHistory(RequestId);
            return new ResultModel<List<RequestsHistory>>(result);
        }
      

    }
}
