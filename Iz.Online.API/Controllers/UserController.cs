using Confluent.Kafka;
using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using model = Izi.Online.ViewModels.Trades;

namespace Iz.Online.API.Controllers
{
    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class UserController : BaseApiController
    {

        #region ctor
        private readonly IExternalUserService _externalUserService;
        private readonly IUserService _userService;

        public UserController(IExternalUserService externalUserService, IUserService userService)
        {
            _externalUserService = externalUserService;
            _userService = userService;
        }

        #endregion
        [HttpGet("Wallet")]
        [EnableCors("CorsPolicy")]
        public ResultModel<Wallet> Wallet([FromBody] ViewBaseModel model)
        {
            var result = _externalUserService.Wallet(new OmsBaseModel()
            {
                Authorization = "",
                UserId = ""
            } /*, GetToken(Request)*/);

            return new ResultModel<Wallet>(result);

        }


        [HttpPost("trade/all")]
        [EnableCors("CorsPolicy")]
        public ResultModel<List<model.Trade>>Trades([FromBody] ViewBaseModel model)
        {
            var result = _userService.Trades(model);
            return new ResultModel<List<model.Trade>>(result);

        }


    }
}
