using System.Dynamic;
using Iz.Online.Services;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Iz.Online.API.Infrastructure
{
    [CustomAuthorization]
    public class BaseApiController : ControllerBase
    {

        protected string JsonSucceed(object data)
        {
            return "";
            //return JsonConvert.SerializeObject(new ResultModel(true, "با موفقیت انجام شد", 0, data));
        }
        protected string JsonError(object? data, int ErrorId = -1, string Msg = "خطایی پیش بینی نشده")
        {
            return "";
            //var r = JsonConvert.SerializeObject(new ResultModel(false, "خطای پیش بینی نشده", -1, data));
            //return r;
        }
    }
}
