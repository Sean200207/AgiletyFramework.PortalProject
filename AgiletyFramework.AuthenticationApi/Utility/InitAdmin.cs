using AgiletyFramework.Commons;
using AgiletyFramework.Commons.EnumEntity;
using AgiletyFramework.DbModels;
using AgiletyFramework.DbModels.Models;

namespace AgiletyFramework.AuthenticationApi.Utility
{
    public static class InitData
    {

        public static void InitAdmin(this WebApplicationBuilder builder)
        {
            //初始化管理员相关信息和权限
            string adminName = builder?.Configuration["Admin:name"];
            string adminPass = builder?.Configuration["Admin:password"];
            if (string.IsNullOrWhiteSpace(adminName) || string.IsNullOrWhiteSpace(adminPass))
            {
                throw new Exception("初始化系统,请配置管理员信息");
            }
            adminPass=MD5Encrypt.Encrypt(adminPass);
            int inltadministratorsid = 0;
            int inltadministratorsRoleid = 0;

            using(AgiletyDbContext context = new AgiletyDbContext(builder.Configuration.GetConnectionString("Default")))
            {
                #region 初始化管理员用户信息
                UserEntity? _User = context.Set<UserEntity>()
                .Where(u=>u.Name.Equals(adminName) && u.UserType == (int)UserTypeEnum.Administrator)
                .FirstOrDefault();

                if (_User != null) 
                {
                    inltadministratorsid = _User.UserId;
                }
                else
                {
                    var userEntity = new UserEntity()
                    {
                        Name = adminName,
                        Password = adminPass,
                        UserType = (int)UserTypeEnum.Administrator,
                        CreateTime = DateTime.Now,
                        Sex = 1,
                        Status = (int)StatusEnum.Normal,
                        QQ = "1234567",
                    };

                    context.Set<UserEntity>().Add(userEntity);
                    inltadministratorsid = userEntity.UserId;
                    context.SaveChanges();
                }
                #endregion

                #region 初始化系统管理员角色
                RoleEntity? _role = context.Set<RoleEntity>()
                    .Where(u=>"系统管理员".Equals(u.RoleName))
                    .FirstOrDefault();

                if(_role != null)
                {
                    inltadministratorsRoleid = _role.RoleId;
                }
                else
                {
                    RoleEntity roleentity = new RoleEntity()
                    {
                        RoleName = "系统管理员",
                        Status = (int)StatusEnum.Normal,
                        ModifyTime = DateTime.Now,
                        CreateTime = DateTime.Now,
                    };

                    context.Set<RoleEntity>().Add(roleentity);
                    inltadministratorsRoleid= roleentity.RoleId;
                    context.SaveChanges();
                }

                #endregion

                #region 初始化管理员--用户角色关联关系
                UserRoleMapEntity? _userRoleMap = context.Set<UserRoleMapEntity>()
                    .Where(c=>c.UserId == inltadministratorsid && c.RoleId == inltadministratorsRoleid)
                    .FirstOrDefault();
                if (_userRoleMap == null)
                {
                    context.Set<UserRoleMapEntity>().Add(new UserRoleMapEntity()
                    {
                        RoleId = inltadministratorsRoleid,
                        UserId = inltadministratorsid,
                        CreateTime = DateTime.Now,
                        ModifyTime = DateTime.Now,
                        Status = (int)StatusEnum.Normal,
                    });
                    context.SaveChanges();
                }
                #endregion

                #region 初始化系统管理员角色--角色和菜单关系
                List<RoleMenuMapEntity> removeMaplist = context.Set<RoleMenuMapEntity>()
                    .Where(c=>c.RoleId==inltadministratorsRoleid)
                    .ToList();
                context.Set<RoleMenuMapEntity>().RemoveRange(removeMaplist);

                List<RoleMenuMapEntity> roleMenuMapEntities = context.Set<MenuEntity>()
                    .Select(a => new RoleMenuMapEntity()
                    {
                        RoleId = inltadministratorsRoleid,
                        MenuId = a.Id,
                        CreateTime = DateTime.Now,
                        ModifyTime = DateTime.Now,
                        Status = (int)StatusEnum.Normal,
                    }).ToList();

                context.Set<RoleMenuMapEntity>().AddRange(roleMenuMapEntities);

                context.SaveChanges();
                #endregion
            }

        }
    }
}
