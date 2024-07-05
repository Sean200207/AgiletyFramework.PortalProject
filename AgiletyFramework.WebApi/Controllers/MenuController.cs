using AgiletyFramework.Commons;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgiletyFramework.WebApi.Controllers
{
    /// <summary>
    /// 菜单的Api
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi =false,GroupName =nameof(ApiVersions.V1))]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IUserService _IUserService;
        private readonly IMenuService _IMenuService;

        private readonly IMapper _IMapper;

        /// <summary>
        /// 控制器 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="iUserService"></param>
        /// <param name="componentContext"></param>
        /// <param name="iMenuService"></param>
        /// <param name="iMapper"></param>
        public MenuController(ILogger<MenuController> logger, IUserService iUserService, Autofac.IComponentContext componentContext, IMenuService iMenuService, IMapper iMapper)
        {
            _logger = logger;
            _IUserService = iUserService;
            _IMenuService = iMenuService;
            _IMapper = iMapper;
        }


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme,Policy ="MenuPolicy")]
        public async Task<JsonResult> GetMenuTreeListAsync()
        {
            //在这里要解析到当前请求的用户是
            //鉴权授权

            //这里能够拿到Userid，说明token必然已经通过验证
            string? strUserId = HttpContext.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrWhiteSpace(strUserId))
            {
                return await Task.FromResult(new JsonResult(new ApiDataResult<int>()
                {
                    Message = "没有token权限",
                    Success = false,
                    OValue = 401
                }));
            }

            var menusTreeList = _IMenuService.GetMenusTreeList(Convert.ToInt32(strUserId));
            var result = new JsonResult(new ApiDataResult<List<MenuTreeDto>>()
            {
                Data = menusTreeList,
                Success = true,
                Message = "获取菜单列表"
            });
            return await Task.FromResult(result);
        }
    }
}
