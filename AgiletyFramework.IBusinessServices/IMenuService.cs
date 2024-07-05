using AgiletyFramework.BusinessServices;
using AgiletyFramework.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.IBusinessServices
{
    public interface IMenuService : IBaseService
    {
        /// <summary>
        /// 根据用户Id计算出用户拥有哪些菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuTreeDto> GetMenusTreeList(int userId);

        //public void Add();
        //public void Deleta();
        //public void Update();
        //public void Query();
    }
}
