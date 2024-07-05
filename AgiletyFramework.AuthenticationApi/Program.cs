using AgiletyFramework.AuthenticationApi.Utility.JwtService;
using AgiletyFramework.BusinessServices;
using AgiletyFramework.DbModels;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AgiletyFramework.WebCore.SwaggerExtend;
using AgiletyFramework.WebCore1.AutoMapExtend;
using AgiletyFramework.WebCore1.CorsExtend;
using Microsoft.EntityFrameworkCore;

namespace AgiletyFramework.AuthenticationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            #region 注册EFCore
            //builder.Services.AddAuthorization();

            builder.Services.AddTransient<AbstractJwtService, JwtHSService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));
            builder.Services.AddDbContext<DbContext, AgiletyDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            #endregion

            #region Swagger
            builder.Services.AddSwaggerExt("敏捷后台管理授权服务器Api文档", "敏捷后台管理授权服务器Api文档");
            #endregion

            #region 引入Autommaper
            builder.Services.AddAutoMapper(typeof(AutoMapConfig));
            #endregion

            #region 配置跨域策略
            builder.Services.AddCorsExt();
            #endregion

            #region 支持MemoryCache
            builder.Services.AddMemoryCache();
            #endregion

            var app = builder.Build();

            app.LoginApi();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                //app.UseSwagger();
                //app.UseSwaggerUI();

                app.UseSwaggerExt("敏捷后台管理项目认证服务器Api文档");
            //}

            //跨域
            app.UseCorsExt();

            app.UseAuthorization();

            
            app.Run();
        }
    }
}