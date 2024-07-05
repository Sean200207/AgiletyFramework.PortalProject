using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.DbModels.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleEntity : BaseModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// 用户状态  0正常 1冻结 2删除
        /// </summary>
        public int Status { set; get; }
    }
}
