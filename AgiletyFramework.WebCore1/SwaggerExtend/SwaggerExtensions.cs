using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgiletyFramework.WebApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

namespace AgiletyFramework.WebCore.SwaggerExtend
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 配置Swagger
        /// </summary>
        /// <param name="service"></param>
        public static void AddSwaggerExt(this IServiceCollection service, string docName, string docDescription)
        {
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen(option =>
            {
                foreach (var version in typeof(ApiVersions).GetEnumNames())
                {
                    option.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = !string.IsNullOrWhiteSpace(docName) ? docName : $"敏捷后台管理项目实战Api文档",
                        Version = version,
                        Description = $"通用版本的CoreApi版本--{version}"
                    });
                }

                //xml文档绝对路径
                var file = Path.Combine(AppContext.BaseDirectory,
                    $"{AppDomain.CurrentDomain.FriendlyName}.xml");
                //true : 显示控制器层注释
                option.IncludeXmlComments(file, true);
                //对action的名称进行排序，如果有多个，就可以看见效果了
                option.OrderActionsBy(o => o.RelativePath);

                #region 支持jwt token授权
                {
                    //添加安全定义--配置支持token授权机制
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "请输入token，格式为Bearer xxxxxx（注意中间必须有空格）",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    //添加安全要求
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference()
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                }
                #endregion
            });
        }

        /// <summary>
        /// 使用Swagger的中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExt(this WebApplication app, string docName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {

                foreach (string varsion in typeof(ApiVersions).GetEnumNames())
                {
                    option.SwaggerEndpoint($"/swagger/{varsion}/swagger.json", string.IsNullOrWhiteSpace(docName) ? 
                        docName : $"敏捷后台管理项目实战Api文档【{varsion}】版本");
                }
            });
        }
    }
}
