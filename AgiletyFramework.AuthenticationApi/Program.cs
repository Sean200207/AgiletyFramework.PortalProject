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

            #region ע��EFCore
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
            builder.Services.AddSwaggerExt("���ݺ�̨������Ȩ������Api�ĵ�", "���ݺ�̨������Ȩ������Api�ĵ�");
            #endregion

            #region ����Autommaper
            builder.Services.AddAutoMapper(typeof(AutoMapConfig));
            #endregion

            #region ���ÿ������
            builder.Services.AddCorsExt();
            #endregion

            #region ֧��MemoryCache
            builder.Services.AddMemoryCache();
            #endregion

            var app = builder.Build();

            app.LoginApi();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                //app.UseSwagger();
                //app.UseSwaggerUI();

                app.UseSwaggerExt("���ݺ�̨������Ŀ��֤������Api�ĵ�");
            //}

            //����
            app.UseCorsExt();

            app.UseAuthorization();

            
            app.Run();
        }
    }
}