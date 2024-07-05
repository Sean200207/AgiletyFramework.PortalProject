using AgiletyFramework.BusinessServices;
using AgiletyFramework.DbModels;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.WebApi.Utility.AutofacExtend;
using AgiletyFramework.WebCore.SwaggerExtend;
using AgiletyFramework.WebCore1.AuthorizationExtend;
using AgiletyFramework.WebCore1.AutoMapExtend;
using AgiletyFramework.WebCore1.CorsExtend;
using AgiletyFramework.WebCore1.DownloadFileExtend;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.X509Certificates;

namespace AgiletyFramework.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region IOC
            //数据库链接字符串
            builder.Services.AddDbContext<DbContext, AgiletyDbContext>(builderOptions =>
            {
                builderOptions.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                
            });
            ////业务逻辑层
            //builder.Services.AddTransient<IUserService, UserService>();

            #endregion

            #region Autofac--并不是提前替换内置的容器，而是和内置的ServiceCollection容器并存的。
            //配置Autofac
            builder.Host.AutofacRegister();
            #endregion

            #region Automapper注册
            builder.Services.AddAutoMapper(typeof(AutoMapConfig));
            #endregion


            //log4Net引入
            builder.Logging.AddLog4Net("CfgFiles/log4net.config");

            builder.Services.AddControllers();


            //跨域
            builder.Services.AddCorsExt();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region Swagger
            builder.Services.AddSwaggerExt("敏捷后台管理项目实战Api文档", "敏捷后台管理项目实战Api文档");
            #endregion

            #region jwt鉴权授权
            builder.RegisterAuthorization();
            #endregion

            var app = builder.Build();

            app.UseCorsExt();

            //拓展一个中间件来读取图片
            app.UseDownloadImages(Directory.GetCurrentDirectory());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt("敏捷后台管理项目实战Api文档");
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}