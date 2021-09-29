using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NineBizlogistics.Model;

namespace NineBizlogistics.Config
{
    public class AuthFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool Check = true;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                Check = !controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                        .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }
            UserInfo user = null;
            if (Check)
            {
                user = TokenHelper.TryGetUser(context.HttpContext.Request);
                if (user == null)
                {
                    OnErr(context, "未授权");
                }
            }
        }
        void OnErr(AuthorizationFilterContext context, string Err)
        {
            JsonModel J = new JsonModel();
            J.SetError(Err);
            var result = JsonConvert.SerializeObject(J);
            context.Result = new ObjectResult(result);
        }
    }
}
