using AgiletyFramework.Commons;
using AgiletyFramework.ModelDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.AuthorizationExtend
{
    public static class AuthorizationExtensions
    {
        public static void RegisterAuthorization(this WebApplicationBuilder builder)
        {
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = InitTokenValidationParameters(tokenOptions);
                    options.Events = new JwtBearerEvents
                    {
                        //此处为权限验证失败后触发的事件，涵盖的场景：没有token的，token错误的场景，封装到InitOnChallenge方法中去
                        OnChallenge = InitOnChallenge,
                        //此处为权限验证失败后触发的事件，涵盖的场景：有token且token是正确的token，但是token涵盖的权限不够，封装到InitOnForbidden方法中去
                        OnForbidden = InitOnForbidden,
                        //此处为权限验证成功后会执行的行为，封装到InitOnTokenValidated方法中去
                        OnTokenValidated = InitOnTokenValidated,
                        //此处为权限验证异常了，封装到InitOnAuthenticationFailed方法中去
                        OnAuthenticationFailed = InitOnAuthenticationFailed,
                        //此处为开始验证权限前夕可以增加的业务逻辑，封装到InitOnMessageReceived方法中去
                        OnMessageReceived = InitOnMessageReceived
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MenuPolicy", policyBuilder => policyBuilder
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .AddRequirements(new MenuAuthorizeRequirement()));
            });
            builder.Services.AddTransient<IAuthorizationHandler, MenuAuthorizeHandler>();
        }

        /// <summary>
        /// 配置默认的参数验证
        /// </summary>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        private static TokenValidationParameters InitTokenValidationParameters(JWTTokenOptions tokenOptions) => new TokenValidationParameters
        {
            //JWT有一些默认的属性，就是给鉴权时就可以筛选了
            ValidateIssuer = true,//是否验证Issuer
            ValidateAudience = true,//是否验证Audience
            ValidateLifetime = true,//是否验证失效时间
            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            ValidAudience = tokenOptions.Audience,
            ValidIssuer = tokenOptions.Issuer,//Issuer，这俩项和前面签发jwt的设置一致
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
            ClockSkew = TimeSpan.FromSeconds(0),//设置token过期后多久失效，默认过期后300秒内仍有效
            //AudienceValidator = (m, n, z) => true, //自定义规则通通不验证，返回true
            //LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => true //自定义规则通通不验证，返回true
        };

        /// <summary>
        /// 此处为权限验证失败后触发的事件，涵盖的场景：没有token的，token错误的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task InitOnChallenge(JwtBearerChallengeContext context)
        {
            //此处代码为终止，Net Core默认的返回类型和数据结果，这个很重要，必须
            context.HandleResponse();
            //自定义自己想要返回的数据结果，这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
            var payload = JsonConvert.SerializeObject(new ApiDataResult<int>()
            {
                Success = false,
                Message = "对不起没有授权，没有Token",
                Data = 0,
                OValue = 401
            }, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            //自定义返回的数据类型
            context.Response.ContentType = "application/json";
            //自定义返回状态码，默认为403，这里改成200
            context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //输出Json数据结果
            context.Response.WriteAsync(payload);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 此处为权限验证失败后触发的事件，涵盖的场景：有token且token是正确的token，但是token涵盖的权限不够
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task InitOnForbidden(ForbiddenContext context)
        {
            //自定义自己想要返回的数据结果，这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
            var payload = JsonConvert.SerializeObject(new ApiDataResult<int>()
            {
                Success = false,
                Message = "对不起，您不具备反问该功能的权限",
                Data = 1,
                OValue = 403
            }, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            //自定义返回的数据类型
            context.Response.ContentType = "application/json";
            //自定义返回状态码，默认为403，这里改成200
            context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //输出Json数据结果
            context.Response.WriteAsync(payload);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task InitOnTokenValidated(TokenValidatedContext context)
        {
            Console.WriteLine("Token解析成功了");
            return Task.FromResult(0);
        }

        /// <summary>
        /// 此处为权限验证异常了
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task InitOnAuthenticationFailed(AuthenticationFailedContext context)
        {
            Console.WriteLine("Token解析成功了");
            return Task.FromResult(0);
        }

        /// <summary>
        /// 此处为开始验证权限前夕可以增加的业务逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task InitOnMessageReceived(MessageReceivedContext context)
        {
            return Task.FromResult(0);
        }

    }
}
