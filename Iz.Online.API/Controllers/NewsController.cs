using Iz.Online.API.Infrastructure;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.News;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class NewsController : BaseApiController
    {
        #region ctor
        private readonly INewsServices _newsServices;

        public NewsController(INewsServices newsServices, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _newsServices = newsServices;
            _newsServices._externalNewsService.Token = _token_;

        }
        #endregion

        [HttpGet("Messages")]
        public async Task<IActionResult> Messages()
        {
            var result = await _newsServices.Messages();
            return new Respond<List<Message>>().ActionRespond(result);
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(string id)
        {
            var result = await _newsServices.Read(id);
            return new Respond<bool>().ActionRespond(result);
        }

        [HttpGet("UnreadMessages")]
        public async Task<IActionResult> UnreadMessages()
        {
            var result = await _newsServices.UnreadMessages();
            return new Respond<MessageIds>().ActionRespond(result);
        }
    }
}
