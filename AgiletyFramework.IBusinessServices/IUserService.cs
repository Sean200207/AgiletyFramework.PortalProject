using AgiletyFramework.AutofacAOP.AopExtend;
using AgiletyFramework.BusinessServices;
using AgiletyFramework.ModelDto;
using Autofac.Extras.DynamicProxy;

namespace AgiletyFramework.IBusinessServices
{
    /// <summary>
    /// 业务逻辑层抽象
    /// </summary>
    [Intercept(typeof(CustomLogInterceptor))]//通过接口方式来完成对于Aop的支持
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 登录功能
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserDto? Login(string userName, string password);

        public void ShowUserAndCompany();

        public void SetUserAndCompany();

        //public void Add();

        //public void Deleta();

        //public void Update();

        //public void Query();

    }
}