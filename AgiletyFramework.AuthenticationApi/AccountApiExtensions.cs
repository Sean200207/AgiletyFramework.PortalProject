using System.Runtime.CompilerServices;
using AgiletyFramework.AuthenticationApi.Utility.JwtService;
using AgiletyFramework.Commons;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.WebCore1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Caching.Memory;
using AgiletyFramework.WebApi;

namespace AgiletyFramework.AuthenticationApi
{
    public static class AccountApiExtensions
    {
        /// <summary>
        /// 登录颁发Token
        /// </summary>
        /// <param name="app"></param>
        public static void LoginApi(this WebApplication app)
        {
            //登录使用用户名和密码获取token
            app.MapPost("auth/Account", ([FromServices] IUserService userManagerService, [FromServices]
                AbstractJwtService _iJWTService, [FromServices] IMemoryCache iMemoryCache, User user) =>
            {
                UserDto? userDto = userManagerService.Login(user.Name, user.Password);
                if (userDto == null)
                {
                    return new ApiDataResult<object>()
                    {
                        Success = false,
                        Message = "用户名或密码错误"
                    };
                }

                //颁发Token
                string accesstoken = _iJWTService.CreateAccessToken(userDto);
                string refreshToken = _iJWTService.CreateRefreshToken(userDto);
                return new ApiDataResult<object>()
                {
                    Success = true,
                    Message = "登陆成功",
                    Data = new
                    {
                        RefreshToken = refreshToken,
                        Accesstoken = accesstoken
                    }
                };
            }).WithGroupName(ApiVersions.V1.ToString());
            //.WithDescription("登录-使用用户名密码获取Token");

            //刷新Token
            app.MapGet("auth/Account", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            ([FromServices] AbstractJwtService _iJWTService, [FromServices] IMemoryCache _IMemoryCache, HttpContext context) =>
            {
                string refreshtokenGuid = context?.User?.FindFirst("refreshtokenGuid")?.Value;
                if (string.IsNullOrWhiteSpace(refreshtokenGuid))
                {
                    return new ApiResult()
                    {
                        Success = false,
                        Message = "已过期需要重新登陆",
                    };
                }
                UserDto userDto = _IMemoryCache.Get<UserDto>(refreshtokenGuid);
                if (userDto == null)
                {
                    return new ApiResult()
                    {
                        Success = false,
                        Message = "已过期需要重新登陆",
                    };
                }
                string token = _iJWTService.CreateAccessToken(userDto);
                return new ApiDataResult<string>()
                {
                    Success = true,
                    Message = "Token刷新成功",
                    Data = token
                };
            }).WithGroupName(ApiVersions.V1.ToString());
            //.WithDescription("刷新Token,使用RreshToken换新Token"); ;
        }
        
    }
}
