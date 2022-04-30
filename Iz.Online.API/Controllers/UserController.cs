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
 
using Izi.Online.ViewModels.Reports;
using CashHelper;

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
        public IActionResult login(CustomerHub model)
        {
            return Ok(new ResultModel<List<AppConfigs>>(null));

        }

        [HttpPost("logOut")]
        public async Task<IActionResult> logOut()
        {
            var result = await _userService.LogOut();
            return new Respond<bool>().ActionRespond(result);

        }

        [HttpGet("captcha")]
        public async Task<IActionResult> Captcha()
        {
            var result = await _userService.Captcha();
            return new Respond<Captcha>().ActionRespond(result);

        }

        [HttpPost("SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] Credentials credentials)
        {
            var result = await _userService.SendOtp(credentials);
            return new Respond<OtpResult>().ActionRespond(result);
        }

        [HttpPost("CheckOtp")]
        public async Task<IActionResult> CheckOtp([FromBody] Otp otp)
        {
            var result = await _userService.CheckOtp(otp);
            return new Respond<CheckedOtp>().ActionRespond(result);

        }

        [HttpGet("CustomerInfo")]
        public async Task<IActionResult> CustomerInfo()
        {
            var result = await _userService.GetCustomerInfo();
            return new Respond<CustomerData>().ActionRespond(result);
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
        public async Task<IActionResult> SetHubId([FromBody] CustomerHub model)
        {
            var result =  await _userService.SetUserHub(_token_, model.HubId);
            return new Respond<bool>().ActionRespond(result);

        }

    

        //get customer wallet.
        [HttpGet("Wallet")]
        public async Task<IActionResult> Wallet()
        {

            var result = await _userService.Wallet();
            return new Respond<Wallet>().ActionRespond(result);

        }

        ////get customer portfolio
        //[HttpGet("portfolio")]
        //public async Task<IActionResult> AllAssets()
        //{
        //    var result = await _userService.AllAssets();
        //    return new Respond<List<Asset>>().ActionRespond(result);
        //} 
        //get customer portfolio
        [HttpGet("portfolio")]
        public async Task<IActionResult> Portfolio()
        {
            var result = await _userService.Portfolio();
            return new Respond<List<Asset>>().ActionRespond(result);
        }

        [HttpGet("portfolioPaged")]
        public async Task<IActionResult> AllAssetsPaged(PortfoFilter filter)
        {
            var result = await _userService.AllAssetsPaged(filter);
            return new Respond<List<Asset>>().ActionRespond(result);
        }
       
    }
}
