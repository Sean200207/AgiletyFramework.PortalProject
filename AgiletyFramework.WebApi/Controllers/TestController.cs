using AgiletyFramework.Commons;
using AgiletyFramework.DbModels;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgiletyFramework.WebApi.Controllers
{

    /// <summary>
    /// 这是一个测试的控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _Logger;
        private readonly DbContext _Context;
        private readonly IUserService _IUserService;

        private readonly IMapper _IMapper;

        public TestController(ILogger<TestController> logger, DbContext context,
            IUserService iUserService, IComponentContext componentContext, IMapper iMapper)
        {
            _Context = context;
            _Logger = logger;
            _Logger.LogInformation($"{this.GetType().FullName} 构造函数执行");
            _Logger.LogWarning($"{this.GetType().FullName} 构造函数执行");
            _IUserService = iUserService;
            _IUserService = componentContext.Resolve<IUserService>();

            _IMapper = iMapper;
        }

        /// <summary>
        /// 这是一个Get Api
        /// </summary>
        /// <returns></returns>
        [HttpGet()]

        public object Get()
        {
            ////数据库链接字符串
            //string connectionString = "Data Source=DESKTOP-TGSR1TT;Initial Catalog=AgiletyFramework.DB;" +
            //    "Persist Security Info=True;User ID=sa;Password=;TrustServerCertificate=true";
            //using (AgiletyDbContext context = new AgiletyDbContext(connectionString))
            //{
            //    //执行查询操作1
            //    UserEntity user = context.UserEntities.OrderByDescending(c => c.UserId).FirstOrDefault();
            //    return new JsonResult(user);
            //}

            //执行查询操作2
            _Logger.LogInformation("执行Get Api查询数据");
            //UserEntity user = _Context.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();
            UserEntity user = _IUserService.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();

            UserEntity user1 = _IUserService.Find<UserEntity>(1);

            //测试Aop
            _IUserService.SetUserAndCompany();
            _IUserService.ShowUserAndCompany();

            return new JsonResult(user);
        }
        /// <summary>
        /// 这是一个Post Api
        /// </summary>
        /// <returns></returns>
        [HttpPost()]

        public IActionResult Post()
        {
            UserEntity user = _IUserService.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();

            //如果需求中是要返回5个字段，但是和数据库映射的实体有10哥字段。如果使用这个实体来返回。必须要返回10个字段。如何做到灵活的返回；

            //Entity 转换成Dto的过程
            //AutomApper
            UserDto userDto = _IMapper.Map<UserEntity, UserDto>(user);

            return new JsonResult(new ApiDataResult<UserDto>()
            {
                Success = true,
                Message = "查询用户",
                Data = userDto,
                OValue = null
            });
        }

        [HttpPut()]

        public IEnumerable<WeatherForecast> Put()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
            })
                .ToArray();
        }

        [HttpDelete()]
        public IEnumerable<WeatherForecast> Delete()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
            })
            .ToArray();
        }
    }
}