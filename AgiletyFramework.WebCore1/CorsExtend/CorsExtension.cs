using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.CorsExtend
{
    public static class CorsExtension
    {
        /// <summary>
        /// 配置跨域策略
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsExt(this IServiceCollection services)
        {
            //跨域
            services.AddCors(options =>
            {
                //allcore:策略名称
                options.AddPolicy("allcors", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                               .AllowAnyOrigin()
                               .AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// 配置跨域策略生效
        /// </summary>
        /// <param name="app"></param>
        public static void UseCorsExt(this WebApplication app)
        {
            app.UseCors("allcors");
        }
    }
}
