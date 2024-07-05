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
    /// ����һ�����ԵĿ�����
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
            _Logger.LogInformation($"{this.GetType().FullName} ���캯��ִ��");
            _Logger.LogWarning($"{this.GetType().FullName} ���캯��ִ��");
            _IUserService = iUserService;
            _IUserService = componentContext.Resolve<IUserService>();

            _IMapper = iMapper;
        }

        /// <summary>
        /// ����һ��Get Api
        /// </summary>
        /// <returns></returns>
        [HttpGet()]

        public object Get()
        {
            ////���ݿ������ַ���
            //string connectionString = "Data Source=DESKTOP-TGSR1TT;Initial Catalog=AgiletyFramework.DB;" +
            //    "Persist Security Info=True;User ID=sa;Password=;TrustServerCertificate=true";
            //using (AgiletyDbContext context = new AgiletyDbContext(connectionString))
            //{
            //    //ִ�в�ѯ����1
            //    UserEntity user = context.UserEntities.OrderByDescending(c => c.UserId).FirstOrDefault();
            //    return new JsonResult(user);
            //}

            //ִ�в�ѯ����2
            _Logger.LogInformation("ִ��Get Api��ѯ����");
            //UserEntity user = _Context.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();
            UserEntity user = _IUserService.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();

            UserEntity user1 = _IUserService.Find<UserEntity>(1);

            //����Aop
            _IUserService.SetUserAndCompany();
            _IUserService.ShowUserAndCompany();

            return new JsonResult(user);
        }
        /// <summary>
        /// ����һ��Post Api
        /// </summary>
        /// <returns></returns>
        [HttpPost()]

        public IActionResult Post()
        {
            UserEntity user = _IUserService.Set<UserEntity>().OrderByDescending(c => c.UserId).FirstOrDefault();

            //�����������Ҫ����5���ֶΣ����Ǻ����ݿ�ӳ���ʵ����10���ֶΡ����ʹ�����ʵ�������ء�����Ҫ����10���ֶΡ�����������ķ��أ�

            //Entity ת����Dto�Ĺ���
            //AutomApper
            UserDto userDto = _IMapper.Map<UserEntity, UserDto>(user);

            return new JsonResult(new ApiDataResult<UserDto>()
            {
                Success = true,
                Message = "��ѯ�û�",
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