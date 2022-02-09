using Confluent.Kafka;
using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("SetHubId")]
        public ResultModel<bool> SetHubId([FromBody] CustomerHub model)
        {

            return new ResultModel<bool>(true);

        }

        [HttpPost("Wallet")]
        public ResultModel<Wallet> Wallet([FromBody] ViewBaseModel model)
        {
            var result = _userService.Wallet(model);
            return new ResultModel<Wallet>(result);
        }

        [HttpPost("portfolio")]
        public ResultModel<List<Asset>> AllAssets([FromBody] ViewBaseModel model)
        {
            var result = _userService.AllAssets(model);
            return new ResultModel<List<Asset>>(result);
        }

      
    }
}
