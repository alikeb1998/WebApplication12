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

        [HttpGet("login")]
        public ActionResult login()
        {
            return Ok(new ResultModel<List<AppConfigs>>(null));
            
        }

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

        //get customer wallet.
        [HttpGet("Wallet")]
        public ResultModel<Wallet> Wallet()
        {
            var result = _userService.Wallet();
            return result;
        }

        //get customer portfolio
        [HttpGet("portfolio")]
        public ResultModel<List<Asset>> AllAssets()
        {
            var result = _userService.AllAssets();
            return result;
        }

        //set a token in database.
        [HttpPost("token/set")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public string Set(string token)
        {
           
            try
            {
                // _userService.SetToken(token);
                var path = @"C:\jafarinejad\store\token.txt";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.Create(path).Close();
                System.IO.File.WriteAllText(path, token);

                return "token is set";
            }
            catch (Exception e)
            {
                return e.Message.ToString() + " ___ " + e.InnerException.Message.ToString();
            }
        }

        //get a token from database.
        [HttpGet("token/get")]
        public string Get()
        {
            var path = @"C:\jafarinejad\store\token.txt";
            if (System.IO.File.Exists(path))
            {
                return  System.IO.File.ReadAllText(path);
            }
            return null;
            
        }

    }
}
