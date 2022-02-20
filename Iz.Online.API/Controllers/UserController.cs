using Confluent.Kafka;
using Iz.Online.API.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Users;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Users;
using Izi.Online.ViewModels.ValidityType;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("captcha")]
        public Captcha Captcha()
        {
            var res = _userService.Captcha();
            return res;

        }

        [HttpPost("SendOtp")]
        public OtpResult SendOtp([FromBody] Credentials credentials)
        {
            var res = _userService.SendOtp(credentials);
            return res;

        }

        [HttpPost("CheckOtp")]
        public CheckedOtp CheckOtp([FromBody] Otp otp)
        {
            var res = _userService.CheckOtp(otp);
            return res;

        }

        [HttpGet("Config")]
        public ResultModel<List<AppConfigs>> Config()
        {
            return new ResultModel<List<AppConfigs>>(_userService.AppConfigs());
        }
        [HttpGet("Validity")]
        public ResultModel<List<ValidityType>> Validity()
        {
            return new ResultModel<List<ValidityType>>(
                new List<ValidityType>() {
                new ValidityType(){
                    Key ="Day",
                    Type = "int",
                    Value = "روز"
                },
                    new ValidityType()
                    {
                        Key ="FillAndKill",
                        Type = "int",
                        Value = "انجام و حذف"
                    },
                new ValidityType()
                {
                    Key ="GoodTillCanceled",
                    Type = "int",
                    Value = "معتبر تا لغو"
                },
                new ValidityType()
                {
                    Key ="GoodTillDate",
                    Type = "DateTime",
                    Value ="معتبر تا تاریخ"
                }
                });
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
                return System.IO.File.ReadAllText(path);
            }
            return null;

        }

    }
}
