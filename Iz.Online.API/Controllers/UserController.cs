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

namespace Iz.Online.API.Controllers
{

    [Produces("application/json")]
    [Route("V1/[controller]")]
    public class UserController : BaseApiController
    {

        #region ctor
        private readonly IUserService _userService;
        //private readonly IDistributedCache _cache;
        //private readonly IConnectionMultiplexer _redis;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor/*, IDistributedCache cache, IConnectionMultiplexer redis*/) : base(httpContextAccessor)
        {
            _userService = userService;
            _userService._token = _token_;
            //_cache = cache;
            //_redis = redis;
        }

        #endregion

        //get a token from database.
        [HttpGet("token/get")]
        public string Get()
        {
            _userService.SetUserHub("new", "h1");
            var t = _userService.UserHubsList("new");
            _userService.SetUserHub("new", "h2");
             t = _userService.UserHubsList("new");
            _userService.SetUserHub("new", "h3");
            t = _userService.UserHubsList("new");
            _userService.SetUserHub("new", "h1");
            t = _userService.UserHubsList("new");
            _userService.SetUserHub("new", "h1");
            t = _userService.UserHubsList("new");
            var r = t;

            var path = @"C:\jafarinejad\store\token.txt";

            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            return null;

        }


        [HttpPost("login")]
        public ActionResult login(CustomerHub model)
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

        [HttpGet("portfolioPaged")]
        public ResultModel<List<Asset>> AllAssetsPaged(PortfoFilter filter)
        {
            var result = _userService.AllAssetsPaged(filter);
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


        [HttpGet("login/check-otp")]
        public ResultModel<string> CheckOtp()
        {
            string token = ""; //TODO get token from oms
            token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJSZXNvdXJjZXMiOlt7IlRhZyI6IkdldEFsbEFzc2V0cyIsIlVyaSI6Ii9vcmRlci9hc3NldC9hbGwiLCJJZCI6NTUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMve3F9IiwiSWQiOjU2LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxJbnN0cnVtZW50cyIsIlVyaSI6Ii9vcmRlci9pbnN0cnVtZW50cyIsIklkIjo1NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0RGFzaGJvYXJkIiwiVXJpIjoiL3VzZXIvZGFzaGJvYXJkIiwiSWQiOjU5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRXYWxsZXQiLCJVcmkiOiIvdXNlci93YWxsZXQiLCJJZCI6NjAsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbE9ic2VydmVyTWVzc2FnZXMiLCJVcmkiOiIvcmxjL29ic2VydmVyLW1lc3NhZ2VzIiwiSWQiOjYxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRMaWdodFdlaWdodEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMtbGlnaHR3ZWlnaHQiLCJJZCI6NjIsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldFRva2VuIiwiVXJpIjoiL3BheW1lbnQiLCJJZCI6MTA4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYWxsQmFjayIsIlVyaSI6Ii9wYXltZW50IiwiSWQiOjEwOSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsRGlzY2xhaW1lcnMiLCJVcmkiOiIvZGlzY2xhaW1lcnMvZ2V0QWxsIiwiSWQiOjExMCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hhbmdlV2l0aFNlc3Npb25EaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2VXaXRoU2Vzc2lvbiIsIklkIjoxMTMsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkU2VuZE90cCIsIlVyaSI6Ii91c2VyL2NoYW5nZS1wYXNzd29yZC9zZW5kLW90cCIsIklkIjoxMTQsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkQ2hlY2tPdHAiLCJVcmkiOiIvdXNlci9jaGFuZ2UtcGFzc3dvcmQvY2hlY2stb3RwIiwiSWQiOjExNSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkT3JkZXIiLCJVcmkiOiIvb3JkZXIvYWRkIiwiSWQiOjQ5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJVcGRhdGVPcmRlciIsIlVyaSI6Ii9vcmRlci91cGRhdGUve2lkOmxvbmd9IiwiSWQiOjUwLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYW5jZWxPcmRlciIsIlVyaSI6Ii9vcmRlci9jYW5jZWwve2lkOmxvbmd9IiwiSWQiOjUxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbC9vcGVuIiwiSWQiOjUyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxBY3RpdmVPcmRlcnMiLCJVcmkiOiIvb3JkZXIvYWxsL2FjdGl2ZSIsIklkIjo1MywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbCIsIklkIjo1NCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiVmVyaWZ5T3JkZXIiLCJVcmkiOiIvb3JkZXIvdmVyaWZ5IiwiSWQiOjU4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxUcmFkZXMiLCJVcmkiOiIvdHJhZGUvYWxsIiwiSWQiOjYzLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT2ZmZXJzIiwiVXJpIjoiL29yZGVyL29mZmVyL29wZW4iLCJJZCI6NjUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFjdGl2ZUlvT3JkZXJzIiwiVXJpIjoiL2lvL2FjdGl2ZSIsIklkIjo2NiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9PcmRlcnMiLCJVcmkiOiIvaW8vb3BlbiIsIklkIjo2NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkSW9PcmRlciIsIlVyaSI6Ii9pby9hZGQiLCJJZCI6NjgsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IlVwZGF0ZUlvT3JkZXIiLCJVcmkiOiIvaW8vdXBkYXRlIiwiSWQiOjY5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJEZWxldGVJb09yZGVyIiwiVXJpIjoiL2lvL2RlbGV0ZSIsIklkIjo3MCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9UcmFkZXMiLCJVcmkiOiIvaW8vdHJhZGVzIiwiSWQiOjcxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDaGFuZ2VEaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2UiLCJJZCI6MTEyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH1dLCJTZXNzaW9uIjoiY2Q4ODM2MTUtOGM3MS00YTQ5LThmNzItOTQyMDlhN2UyNmE2IiwiVXNlcm5hbWUiOiJzYXR0YXIiLCJFbWFpbEFkZHJlc3MiOiJzYXR0YXJAc2NlbnVzLmNvbSIsIk1vYmlsZU51bWJlciI6Iis5ODkxNjYxNTg5MzUiLCJJZCI6MjEsIlBhc3N3b3JkRXhwaXJhdGlvbiI6IjIwMjItMDgtMDhUMTY6MDA6MjkuNzk3IiwiZXhwIjoxNjQ1NDMxMzQ5fQ.7iiR7CwdJsS4so_vk1lpGYSrwnMr9JUDdaRJQ8U0bLw";
            return _userService.GetUserLocalToken(token);
            //return new ResultModel<string>(Guid.NewGuid().ToString());
        }


    }
}
