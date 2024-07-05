using AgiletyFramework.Commons;
using AgiletyFramework.Commons.EnumEntity;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgiletyFramework.WebApi.Controllers
{
    /// <summary>
    /// Api控制器，用户相关的Api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _Logger;
        private readonly IUserService _IUserService;
        private readonly IMapper _IMapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="iUserService"></param>
        /// <param name="iMapper"></param>
        public UserController(ILogger<UserController> logger,
            IUserService iUserService, IMapper iMapper)
        {
            _Logger = logger;
            _IUserService = iUserService;
            _IMapper = iMapper;
        }

        /// <summary>
        /// 获取用户的分页列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchaString"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageindex:int}/{pageSize:int}")]
        [Route("{pageindex:int}/{pageSize:int}/{searchaString}")]
        public async Task<JsonResult> GetUserPageAsync(int pageindex, int pageSize,
            string? searchaString = null)
        {
            PagingData<UserEntity> paging = _IUserService
                .QueryPage<UserEntity, DateTime>(!string.IsNullOrWhiteSpace(searchaString) ? c =>
                c.Name.Contains(searchaString) : a => true, pageSize, pageindex, c => c.CreateTime, false);

            PagingData<UserDto>  pagingResult = _IMapper.Map<PagingData<UserEntity>,
                PagingData<UserDto>>(paging);

            JsonResult result = new JsonResult(new ApiDataResult<PagingData<UserDto>>()
            {
                Data = pagingResult,
                Success = true,
                Message = "用户分页列表"
            });
            return await Task.FromResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddUserAsync([FromBody] AddUserDto userDto)
        {
            UserEntity adduser = _IMapper.Map<AddUserDto, UserEntity>(userDto);
            adduser.Password = MD5Encrypt.Encrypt(adduser.Password);
            adduser.Status = userDto.IsEnabled ? (int)StatusEnum.Normal : (int)StatusEnum.Frozen;
            adduser.UserType = (int)UserTypeEnum.GeneralUser;
            UserEntity user = _IUserService.Insert(adduser);
            var result = new JsonResult(new ApiDataResult<UserEntity>()
            {
                Data = adduser,
                Success = true,
                Message = "添加用户"
            });
            if (user.UserId <= 0)
            {
                result = new JsonResult(new ApiDataResult<UserEntity>() {
                    Data = adduser,
                    Success = false,
                    Message = "添加用户失败"
                });
            }
            return await Task.FromResult(result);
        }

    }
}
