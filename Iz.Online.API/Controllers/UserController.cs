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
using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;
using System.Dynamic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis;
using Iz.Online.API.Controllers;
using Iz.Online.HubHandler.IServices;
using Izi.Online.ViewModels.Reports;

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class UserController : BaseApiController
    {

        #region ctor
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        public UserController(IUserService userService, ICacheService cacheService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _userService = userService;
            _cacheService = cacheService;
            _userService._externalUserService.Token = _token_;
            
        }

        #endregion
        
        [HttpPost("login")]
        public ActionResult login(CustomerHub model)
        {
            return Ok(new ResultModel<List<AppConfigs>>(null));

        }

        [HttpPost("logOut")]
        public ResultModel<bool> logOut()
        {
            return _userService.LogOut();

        }

        [HttpGet("captcha")]
        public ResultModel<Captcha> Captcha()
        {
            var res = _userService.Captcha();
            return res;

        }

        [HttpPost("SendOtp")]
        public ResultModel<OtpResult> SendOtp([FromBody] Credentials credentials)
        {
            var res = _userService.SendOtp(credentials);
            return res;

        }

        [HttpPost("CheckOtp")]
        public ResultModel<CheckedOtp> CheckOtp([FromBody] Otp otp)
        {
            var res = _userService.CheckOtp(otp);
            SetToken(res.Model.Token);
            return res;

        }

        [HttpGet("CustomerInfo")]
        public ResultModel<CustomerData> CustomerInfo()
        {
            return _userService.GetCustomerInfo();
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
                    Value = "روز",
                    Code = 1
                },
                new ValidityType()
                {
                    Key ="FillAndKill",
                    Type = "int",
                    Value = "انجام و حذف",
                    Code = 4
                },
                new ValidityType()
                {
                    Key ="GoodTillCanceled",
                    Type = "int",
                    Value = "معتبر تا لغو",
                    Code = 3
                },
                new ValidityType()
                {
                    Key ="GoodTillDate",
                    Type = "DateTime",
                    Value ="معتبر تا تاریخ",
                    Code = 2
                }
                });
        }

        [HttpPost("SetHubId")]
        public ResultModel<bool> SetHubId([FromBody] CustomerHub model)
        {
            var result = _userService.SetUserHub(_token_, model.HubId);
            return result;

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

        [HttpGet("portfolioPaged")]
        public ResultModel<List<Asset>> AllAssetsPaged(PortfoFilter filter)
        {
            var result = _userService.AllAssetsPaged(filter);
            return result;
        }
        //set a token in database.
  
        private static string SetToken(string token)
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
    }
}
