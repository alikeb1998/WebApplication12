using Confluent.Kafka;
using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class UserController : BaseApiController
    {
        #region ctor

        private readonly IExternalUserService _externalUserService;

        public UserController(IExternalUserService externalUserService)
        {
            _externalUserService = externalUserService;
        }
    #endregion
        [HttpGet("Wallet")]
        public ResultModel<Wallet> Wallet([FromBody] ViewBaseModel model)
        {
            var result = _externalUserService.Wallet(new OmsBaseModel()
            {
                Authorization = "",
                UserId = ""
            });

            return new ResultModel<Wallet>(result);

        }

    



    }
}
