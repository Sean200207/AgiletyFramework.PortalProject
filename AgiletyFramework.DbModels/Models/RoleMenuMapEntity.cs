using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.DbModels.Models
{
    public class RoleMenuMapEntity : BaseModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public Guid MenuId { get; set; }
    }
}
