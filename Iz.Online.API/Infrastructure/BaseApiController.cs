using System.Dynamic;
using Iz.Online.Services;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Iz.Online.API.Infrastructure
{
    [CustomAuthorization]
    //
    // [EnableCors("CustomCors")]

    public class BaseApiController : ControllerBase
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        public static string _token_;
        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
            _token_ = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            //IConfiguration _configuration = configuration;
            //var conf = _configuration.GetSection("IsDevelopment").Get<string>();
            //var tk = _configuration.GetSection("TK").Get<string>();
            //if (conf == "true")
            //{
            //    _token_ = tk;
            //}
        }

    }
}
