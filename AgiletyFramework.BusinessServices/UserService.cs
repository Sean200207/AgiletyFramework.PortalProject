using AgiletyFramework.AutofacAOP.AopExtend;
using AgiletyFramework.Commons;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgiletyFramework.BusinessServices
{
    /// <summary>
    /// 业务逻辑层实现
    /// </summary>

    [Intercept(typeof(CustomLogInterceptor))]
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _IMapper;
        public UserService(DbContext context,IMapper iMapper) : base(context)
        {
            _IMapper = iMapper;
        }

        /// <summary>
        /// 登录功能
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserDto? Login(string userName, string password)
        {
            string pwd = MD5Encrypt.Encrypt(password);
            List<UserEntity> userList = Context
                .Set<UserEntity>()
                .Where(c => c.Name.Equals(userName) && c.Password.Equals(pwd))
                .ToList();
            if (userList == null || userList.Count <= 0)
            {
                return null;
            }
            UserEntity user = userList.First();
            UserDto userDto = _IMapper.Map<UserEntity, UserDto>(user);

            List<int> roleIdList = Context
                .Set<UserRoleMapEntity>()
                .Where(c => c.UserId == user.UserId)
                .Select(r => r.RoleId).ToList();
            userDto.RoleIdList = roleIdList;//设置登录用户角色

            //登录用户的菜单id
            List<Guid> userMenuIds = Context
                .Set<RoleMenuMapEntity>()
                .Where(c=>roleIdList.Contains(c.Id))
                .Select(c => c.MenuId).ToList();
            return userDto;
        }

        /// <summary>
        /// Autofac支持Aop扩展，如果通过类的方式来支持Aop，只有定义成Virtual方法，才能够进入到Aop内部去
        /// </summary>
        public virtual void SetUserAndCompany()
        {
            
        }

        public void ShowUserAndCompany()
        {

        }

        //public void Add()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Deleta()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Query()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update()
        //{
        //    throw new NotImplementedException();
        //}
    }
}