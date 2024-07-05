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
            //���ݿ������ַ���
            builder.Services.AddDbContext<DbContext, AgiletyDbContext>(builderOptions =>
            {
                builderOptions.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                
            });
            ////ҵ���߼���
            //builder.Services.AddTransient<IUserService, UserService>();

            #endregion

            #region Autofac--��������ǰ�滻���õ����������Ǻ����õ�ServiceCollection��������ġ�
            //����Autofac
            builder.Host.AutofacRegister();
            #endregion

            #region Automapperע��
            builder.Services.AddAutoMapper(typeof(AutoMapConfig));
            #endregion


            //log4Net����
            builder.Logging.AddLog4Net("CfgFiles/log4net.config");

            builder.Services.AddControllers();


            //����
            builder.Services.AddCorsExt();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region Swagger
            builder.Services.AddSwaggerExt("���ݺ�̨������ĿʵսApi�ĵ�", "���ݺ�̨������ĿʵսApi�ĵ�");
            #endregion

            #region jwt��Ȩ��Ȩ
            builder.RegisterAuthorization();
            #endregion

            var app = builder.Build();

            app.UseCorsExt();

            //��չһ���м������ȡͼƬ
            app.UseDownloadImages(Directory.GetCurrentDirectory());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt("���ݺ�̨������ĿʵսApi�ĵ�");
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}