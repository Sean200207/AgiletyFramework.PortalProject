using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.ModelDto
{
    public class MenuTreeDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuText { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string? WebUrlName { get; set; }

        /// <summary>
        /// 前端Url地址--路由的地址s
        /// </summary>
        public string? WebUrl { get; set; }

        /// <summary>
        /// 保存Vue具体文件的某一个地址
        /// </summary>
        public string? VueFilePath { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 递归类型
        /// </summary>
        public List<MenuTreeDto>? Children { get; set; }
    }
}
