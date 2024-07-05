using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.AutofacAOP.AopExtend
{
    public class CustomLogInterceptor : IInterceptor
    {
        private readonly ILogger<CustomLogInterceptor> _Logger;
        public CustomLogInterceptor(ILogger<CustomLogInterceptor> logger)
        {
            _Logger = logger;
        }
        /// <summary>
        /// AOP能够实现的动作
        /// </summary>
        /// <param name="invocation"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Intercept(IInvocation invocation)
        {
            {
                _Logger.LogInformation($"======={invocation.Method.Name} 执行前~~=====");
            }
            invocation.Proceed();//继续往后执行
            {
                _Logger.LogInformation($"======={invocation.Method.Name} 执行后~~=====");
            }
        }
    }
}
