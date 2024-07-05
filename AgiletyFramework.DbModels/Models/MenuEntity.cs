using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.DbModels.Models
{
    public class MenuEntity:BaseModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuText { get; set; }

        public int MenuType { get; set; }

        public string? Icon { get; set; }

        public string? WebUrlName { get; set; }

        public string? WebUrl { get; set; }

        public string? VueFilePath { get; set; }

        public bool IsLeafNode { get; set; }

        public int OrderBy { get; set; }
    }
}
