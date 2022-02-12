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


        [HttpGet("Config")]
        public ResultModel<List<AppConfigs>> Config()
        {
            return new ResultModel<List<AppConfigs>>( _userService.AppConfigs());
        }

        [HttpPost("SetHubId")]
        public ResultModel<bool> SetHubId([FromBody] CustomerHub model)
        {

            return new ResultModel<bool>(true);

        }

        [HttpPost("Wallet")]
        public ResultModel<Wallet> Wallet([FromBody] ViewBaseModel model)
        {
            var result = _userService.Wallet(model);
            return result;
        }

        [HttpPost("portfolio")]
        public ResultModel<List<Asset>> AllAssets([FromBody] ViewBaseModel model)
        {
            var result = _userService.AllAssets(model);
            return result;
        }

        [HttpPost("token/set")]
        public string Set(ViewBaseModel model)
        {
            try
            {
                System.IO.File.WriteAllText(@"C:\jafarinejad\store\token.txt", @$"{model.Token}");
                return model.Token;
                return "token is set";
            }
            catch (Exception e)
            {
                return e.Message + " ___ " + e.InnerException;
            }
        }

        [HttpGet("token/get")]
        public string Get()
        {
            return System.IO.File.ReadAllText(@"C:\jafarinejad\store\token.txt");
        }

    }
}
