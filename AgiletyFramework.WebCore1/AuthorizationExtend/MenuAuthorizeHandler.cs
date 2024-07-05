using AgiletyFramework.IBusinessServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.AuthorizationExtend
{
    public class MenuAuthorizeHandler:AuthorizationHandler<MenuAuthorizeRequirement>
    {
        private readonly IUserService _IUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iUserService"></param>
        public MenuAuthorizeHandler(IUserService iUserService)
        {
            _IUserService = iUserService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MenuAuthorizeRequirement requirement)
        {
            //if (context.User.Claims == null || context.User.Claims.Count() <= 0)
            //{
            //    context?.Fail();
            //}
            //else if (context.User.IsInRole("admin"))
            //{
                context?.Succeed(requirement); //只要执行这句话就表示验证通过了
            //}
            //else
            //{
                //HttpContext httpcontext = (HttpContext)context.Resource!;

                ////通过token中包含的按钮权限来校验
                //{
                //    List<string> currentUserBthList = httpcontext.User.Claims
                //        .Where(c => c.Type.Equals("Btns"))
                //        .Select(c => c.Value)
                //        .ToList();
                //    object? controllerName = httpcontext.GetRouteValue("controller");
                //    object? actionName = httpcontext.GetRouteValue ("action");

                //    bool iContains = currentUserBthList
                //        .Any(m => m.Equals($"{controllerName}-{actionName}",
                //        StringComparison.OrdinalIgnoreCase));
                //    if (iContains)
                //    {
                //        context?.Succeed(requirement);//只要执行这句话就表示验证通过了
                //    }
                //    else
                //    {
                //        context?.Fail();
                //    }

                //    //解析到用户信息后，通过用户的id 去查询下用户具备哪些权限
                //    //链接数据库了
                //    {
                //        string? strUserId = httpcontext.User?.Claims?
                //            .FirstOrDefault(c=>c.Type.Equals(ClaimTypes.Sid))?
                //            .Value;
                //        if (strUserId == null)
                //        {
                //            context?.Fail();
                //        }
                //        else
                //        {
                //            object? controllerName = httpcontext.GetRouteValue("controller");
                //            object? actionName = httpcontext.GetRouteValue("action");
                //            bool bResult = true;
                //            //bool bReuslt = await _IUserService.ValidateBtnAsync(Convert.ToInt32(strUserId),$"{controllerName}-{actionName}");
                //            if (bResult)
                //            {
                //                context?.Succeed(requirement);
                //            }
                //            else
                //            {
                //                context?.Fail();
                //            }
                //        }
                        
                //    }
                //}
            //}

            await Task.CompletedTask;
        }
    }
}
