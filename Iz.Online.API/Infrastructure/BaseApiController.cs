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
          _token_ = "321token123";
        }

    }
}
