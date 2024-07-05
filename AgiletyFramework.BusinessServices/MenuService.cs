using AgiletyFramework.BusinessServices;
using AgiletyFramework.DbModels.Models;
using AgiletyFramework.ModelDto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.IBusinessServices
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IMapper _IMapper;
        public MenuService(DbContext context, IMapper imapper) : base(context)
        {
            _IMapper = imapper;
        }

        /// <summary>
        /// 根据用户Id计算出用户拥有哪些菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuTreeDto> GetMenusTreeList(int userId)
        {
            //根据用户Map的菜单
            //List<int> roleIds = Context.Set<UserRoleMapEntity>()
            //    //.Where(u => u.UserId == userId)
            //    .Select(c => c.RoleId).ToList();

            //List<Guid> menuGuids = Context.Set<RoleMenuMapEntity>()
            //    .Where(u => roleIds.Contains(u.RoleId))
            //    .Select(c => c.MenuId).ToList();

            List<MenuEntity> menuList = Context.Set<MenuEntity>()
                //.Where(m => menuGuids.Contains(m.Id))
                .ToList();

            List<MenuTreeDto> menuDtos = _IMapper.Map<List<MenuEntity>, List<MenuTreeDto>>(menuList);

            return menuDtos;
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
