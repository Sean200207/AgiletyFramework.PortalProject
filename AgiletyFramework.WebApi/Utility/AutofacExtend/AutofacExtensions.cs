using AgiletyFramework.BusinessServices;
using AgiletyFramework.IBusinessServices;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AgiletyFramework.AutofacAOP.AopExtend;

namespace AgiletyFramework.WebApi.Utility.AutofacExtend
{
    /// <summary>
    /// Autofac的扩展
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// 配置的Autofac的注册
        /// </summary>
        /// <param name="host"></param>
        public static void AutofacRegister(this ConfigureHostBuilder host)
        {
            //指定Provider的工厂为AutofacServiceProviderFactory
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            host.ConfigureContainer<ContainerBuilder>(ConfigurationBinder =>
            {
                //在这里就可以注册IOC中  抽象和具体之间的关系
                ConfigurationBinder.RegisterType<UserService>().As<IUserService>()
                //通过接口来支持
                //.EnableInterfaceInterceptors();//unget: Autofac.Extras.DynamicProxy

                //通过类来支持Aop
                .EnableClassInterceptors();

                ConfigurationBinder.RegisterType<CustomLogInterceptor>();

                ConfigurationBinder.RegisterType<MenuService>().As<IMenuService>();
            });
        }
    }
}
