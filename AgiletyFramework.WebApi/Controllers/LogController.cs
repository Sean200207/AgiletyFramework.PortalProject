using AgiletyFramework.Commons;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgiletyFramework.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IUserService _IUserService;
        private readonly IMapper _IMapper;  //AutoMapper映射使用

        public LogController(ILogger<LogController> logger, IUserService iUserService, IMapper iMapper)
        {
            _logger = logger;
            _IUserService = iUserService;
            _IMapper = iMapper;
        }

        /// <summary>
        /// 获取系统日志分页列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchaString"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageindex:int}/{pageSize:int}")]
        [Route("{pageindex:int}/{pageSize:int}/{searchaString}")]
        public async Task<JsonResult> GetUserPageAsync(int pageindex,int pageSize, string? searchaString = null)
        {
            PagingData<SystemLog> paging = _IUserService
                .QueryPage<SystemLog, DateTime>(!string.IsNullOrWhiteSpace(searchaString) ? c =>
                c.Message.Contains(searchaString) : a => true, pageSize, pageindex, c => c.Date, false);

            PagingData<SystemLogDto> pagingResult = _IMapper.Map<PagingData<SystemLog>,
                PagingData<SystemLogDto>>(paging);
            JsonResult result = new JsonResult(new ApiDataResult<PagingData<SystemLogDto>>()
            {
                Data = pagingResult,
                Success = true,
                Message = "日志分页列表"
            });
            return await Task.FromResult(result);
        }

    }
}
