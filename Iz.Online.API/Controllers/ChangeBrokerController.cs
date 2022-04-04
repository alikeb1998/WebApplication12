using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ChangeBroker;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{

    //[Produces("application/json")]
    //[Route("V1/[controller]")]

    //public class ChangeBrokerController : BaseApiController
    //{
    //    private readonly IChangeBrokerService _changeBrokerService;
    //    public ChangeBrokerController(IHttpContextAccessor httpContextAccessor, IChangeBrokerService brokerService) : base(httpContextAccessor)
    //    {
    //        _changeBrokerService = brokerService;
    //    }
       
    //    [HttpGet("AllRequests")]
    //    public async Task<IActionResult> AllRequests()
    //    {
    //        var result = await _changeBrokerService.AllRequests();
    //        return new Respond<List<Request>>().ActionRespond(result);
    //    } 
    //    [HttpPost("RequestDetails")]
    //    public async Task<IActionResult> RequestDetails(long RequestId)
    //    {
    //        var result = await _changeBrokerService.RequestDetails(RequestId);
    //        return new Respond<Request>().ActionRespond(result);
    //    }

    //    [HttpPost("GetDoc")]
    //    public async Task<IActionResult> GetDoc([FromBody] long DocumentId)
    //    {
    //        var result = await _changeBrokerService.GetDocument(DocumentId);
    //        //return new FileContentResult(byteArray, "application/octet-stream");
    //        return new Respond<byte[]>().ActionRespond(result);

    //    }

    //    [HttpPost("AddRequest")]
    //    public async Task<IActionResult> AddRequest([FromBody] NewRequest model)
    //    {
    //        var result = await _changeBrokerService.AddRequest(model);
    //        return new Respond<long>().ActionRespond(result);
    //    }
      
    //    [HttpPost("EditRequest")]
    //    public async Task<IActionResult> EditRequest([FromBody] NewRequest model)
    //    {
    //        var result = await _changeBrokerService.EditRequest(model);
    //        return new Respond<bool>().ActionRespond(result);
    //    }
      
    //    [HttpPost("DeleteRequest")]
    //    public async Task<IActionResult> DeleteRequest([FromBody] long RequestId)
    //    {
    //        var result = await _changeBrokerService.DeleteRequest(RequestId);
    //        return new Respond<bool>().ActionRespond(result);
    //    }
      
        
    //    [HttpPost("RequestHistory")]
    //    public async Task<IActionResult> RequestHistory([FromBody] long RequestId)
    //    {
    //        var result = await _changeBrokerService.RequestHistory(RequestId);
    //        return new Respond<List<RequestsHistory>>().ActionRespond(result);
    //    }
      

    //}
}
