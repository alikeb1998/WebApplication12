using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Iz.Online.Services
{
    public class CustomAuthorization : ActionFilterAttribute
    {
        public override object TypeId => base.TypeId;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool Match(object? obj)
        {
            return base.Match(obj);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            //context.ActionArguments.ToList().First().Value;
            //((Izi.Online.ViewModels.ShareModels.ViewBaseModel) context.ActionArguments.ToList().First().Value).Token =
            // context.HttpContext.Request.Headers["test1"].ToString();


            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtCustomAuthorize : Attribute, IAuthorizationFilter
    {
        string Role;
        public JwtCustomAuthorize(string role)
        {
            Role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //context.Result = new JsonResult(new
            //{ Message = "Token Validation Has Failed. Request Access Denied" }  )
            //{ StatusCode = StatusCodes.Status401Unauthorized};
            //return;

            var method = context.HttpContext.Request.RouteValues["Action"];
            var controler = context.HttpContext.Request.RouteValues["controller"];
            var token = context.HttpContext.Request.Headers["tooken"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new
                { Message = "Token Validation Has Failed. Request Access Denied" })
                { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var deserializedToken = TestJwtSecurityTokenHandler(token);

            var hasAccess = ((JwtSecurityToken)deserializedToken).Claims.Any(x => x.Type == "Roles" && x.Value == Role);

            if (!hasAccess)
            {
                context.Result = new JsonResult(new
                { Message = "Token Validation Has Failed. Request Access Denied" })
                { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
        }

        public SecurityToken TestJwtSecurityTokenHandler(string stream)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            return jsonToken;
        }
    }
}
